import CryptoJS from 'crypto-js';
import HelperMixin from '../../Shared/HelperMixin.vue';
import VueRecaptcha from 'vue-recaptcha';

export default {
    name: 'Login',
    mixins: [HelperMixin],
    components: {
        'vue-recaptcha': VueRecaptcha
    },
    created() {
        //this.$blockUI.$loading = this.$loading;
        this.logout();
    },
    data() {

        //var validatePass = (rules, value, callback) => {
        //    if (value === '') {
        //        callback(new Error('الرجاء إدخال كلمة المرور'));
        //    } else {
        //        if (this.ruleForm.ConfimPassword !== '') {
        //            this.$refs.ruleForm.validateField('ConfimPassword');
        //        }
        //        callback();
        //    }
        //};

        return {

            sitekey: '6LdiM_woAAAAAP8QkkTF_lpFcgvH_iiQNHBZ88s8',
            
            state:0,
            isAuthenticated: false,
            isActive: false,
            form: {
                Password: null,
                Email: null,
                
            },


            ruleForm: {

                Email: '',
                Password: '',
                CaptchaVerify: '',
            },
            rules: {
                Email: [
                    { required: true, message: 'الرجاء إدخال اسم الدخول', trigger: 'blur' },
                    /*{ required: true, pattern: /^[A-Za-z]{0,40}$/, message: 'الرجاء إدخال اسم الدخول بطريقه صحيحه', trigger: 'blur' }*/
                ],
                //Password: [
                //    { validator: validatePass, trigger: 'blur' },
                //    { required: true, pattern: /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]){8,}.*$/, message: '  تتكون كلمة المرور من حروف صغيرة وكبيرو وأرقام', trigger: 'blur' }
                //],
            },

            PhoneNumber:'',


        };
    },
    methods: {

        href(url) {
            this.$router.push(url);
        },

        encrypt: function encrypt(data, SECRET_KEY) {
            var dataSet = CryptoJS.AES.encrypt(JSON.stringify(data), SECRET_KEY);
            dataSet = dataSet.toString();
            return dataSet;
        },
        decrypt: function decrypt(data, SECRET_KEY) {
            data = CryptoJS.AES.decrypt(data, SECRET_KEY);
            data = JSON.parse(data.toString(CryptoJS.enc.Utf8));
            return data;
        },


        onVerify: function (response) {
            this.ruleForm.CaptchaVerify = response;
        },
        onExpired: function () {
        },
        resetRecaptcha() {
            this.$refs.recaptcha.reset() 
        },


        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {


                    this.$blockUI.Start();
                    this.$http.Login(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.resetForm(formName);
                            localStorage.setItem('currentUser-client', this.encrypt(JSON.stringify(response.data), this.PlatFormPass));
                            window.location.href = '/';


                            //this.$helper.ShowMessage('success', ' تمت عملية التسجيل بنجاح', response.data);
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('error', 'خطأ بعملية الاضافة', err.response.data);
                        });


                    //if (!this.ruleForm.CaptchaVerify) {
                    //    this.$helper.ShowMessage('error', 'خطــاء في التحقق من البيانات', 'الـرجاء التحقق من انك لست روبوت');
                    //} else {
                        
                    //}

                } else {
                    this.$helper.showSwal('warning');
                    return false;
                }
            });
        },

        resetForm(formName) {
            this.$refs[formName].resetFields();
        },


        logout() {
            localStorage.removeItem('currentUser-client');
            document.cookie.split(";").forEach(function (c) { document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/"); });
            this.$blockUI.Start();
            this.$http.Logout()
                .then(() => {
                    this.$blockUI.Stop();
                    //  window.location.href = "/Login";
                })
                .catch((err) => {
                    this.$blockUI.Stop(err);
                    //console.error(err);
                });
        },

        login() {
            if (!this.form.Email) {
                this.$notify({
                    title: 'خطأ',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + 'الرجاء إدخال البريد الإلكتروني' + '</strong>',
                    type: 'error'
                });
                return;
            }
            if (!this.form.Password) {
                this.$notify({
                    title: 'خطأ',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + 'الرجاء إدخال الرقم السري' + '</strong>',
                    type: 'error'
                });
                return;
            }

            //this.Loading = true;
            this.$blockUI.Start();
            this.$http.login(this.form)
                .then(response => {
                    //debugger;
                    this.$blockUI.Stop();
                    localStorage.setItem('currentUser-client', JSON.stringify(response.data));
                    window.location.href = '/';
                })
                .catch((error) => {
                    this.$blockUI.Stop();
                    //$blockUI.close();
                    //debugger;
                    // this.Loading = false;
                    this.$notify({
                        title: 'خطأ',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>' + error.response.data + '</strong>',
                        type: 'error'
                    });
                });
        },






        ForgerPassword() {
            this.state = 1;
        },

        SendOtp() {
            if (!this.PhoneNumber) {
                this.$notify({
                    title: 'خطأ',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + 'الرجاء إدخال رقم الهاتف بطريقة صحيحة' + '</strong>',
                    type: 'error'
                });
                return;
            }

        },

        back() {
            this.state = 0;
        }
    }
}
