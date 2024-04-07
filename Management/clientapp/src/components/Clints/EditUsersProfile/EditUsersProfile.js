import moment from 'moment';
import Swal from 'sweetalert2'
import HelperMixin from '../../../Shared/HelperMixin.vue'
export default {
    name: 'EditUsersProfile',
    mixins: [HelperMixin],

    async created() {
        await this.CheckLoginStatus();

        this.ruleForm.LoginName = this.loginDetails.loginName;
        this.ruleForm.Name = this.loginDetails.name;
        this.ruleForm.Email = this.loginDetails.email;
        this.ruleForm.Phone = this.loginDetails.phone;

     
    },
    data() {  
       
        return {

            loginDetails: '',
            ImageName:'',
            fileBase64:'',
            ImageType: '',



            ruleForm: {
                Name: '',
                LoginName: '',
                Email: '',
                Phone: '',
            },
            rules: {
                Name: this.$helper.ArabicOnly(),
                LoginName: this.$helper.EnglishOnly(),
                Email: this.$helper.EmailOnly(),
                Phone: this.$helper.NumberOnlyRequired(),
            },


            ChangePasswordruleForm: {
                Password: '',
                NewPassword: '',
                ConfimPassword: '',
            },
           
            ChangePasswordrules: {
                Password: this.$helper.Required(),
                NewPassword: this.$helper.Required(),
                ConfimPassword: this.$helper.Required(),
            },

            ruleFormAttahcment: {
                Id: '',
                ImageName: '',
                ImageType: '',
                fileBase64: '',
            },
        }
    },

    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
            // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format('MMMM Do YYYY');
        }
    },

    methods: {


        SelectAttachment(file) {
            let str = file.raw.type;
            let type2 = str.substring(0, 5);

            if (type2 != "image") {
                this.$helper.ShowMessage('error', 'خطأ بالعملية', 'الرجاء التأكد من نوع الملف');
            }
            var $this = this;
            var reader = new FileReader();
            reader.readAsDataURL(file.raw);
            reader.onload = function () {
                $this.ruleFormAttahcment.ImageName = file.raw.name;
                $this.ruleFormAttahcment.ImageType = file.raw.type;
                $this.ruleFormAttahcment.fileBase64 = reader.result;
            };
            this.$blockUI.Start();
            this.$http.UploadImage(this.ruleFormAttahcment)
                .then(response => {
                    this.$blockUI.Stop();
                    this.$helper.ShowMessage('success', 'عملية ناجحة ', response.data);
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$helper.ShowMessage('error', 'خطأ بعملية الإضافة', err.response.data);
                });
        },


        //FileChanged(e) {
        //    var files = e.target.files;

        //    if (files.length <= 0) {
        //        return;
        //    }

        //    if (files[0].type !== 'image/jpeg' && files[0].type !== 'image/png') {
        //        this.$message({
        //            type: 'error',
        //            message: 'عفوا يجب انت تكون الصورة من نوع JPG ,PNG'
        //        });
        //        this.photo = null;
        //        return;
        //    }

        //    var $this = this;

        //    var reader = new FileReader();
        //    reader.onload = function () {
        //        $this.photo = reader.result;
        //        $this.UploadImage();
        //    };
        //    reader.onerror = function () {
        //        $this.photo = null;
        //    };
        //    reader.readAsDataURL(files[0]);
        //},

        //UploadImage() {

        //    this.$blockUI.Start();
        //    var obj = {
        //        ImageName: this.photo,
        //        fileBase64: this.photo,
        //        Id: this.loginDetails.id,
        //        ImageType: this.loginDetails.id,
        //    };
        //    this.$http.UploadImage(obj)
        //        .then(response => {
        //            this.$blockUI.Stop();
        //            this.$message({
        //                type: 'info',
        //                message: response.data
        //            });

        //            setTimeout(() =>
        //                window.location.href = '/'
        //                , 500);

        //        })
        //        .catch((err) => {
        //            this.$blockUI.Stop(err);
        //            this.pages = 0;
        //        });
        //},

        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.EditUsersProfile(formName);
                } else {
                    return false;
                }
            });
        },
        resetForm(formName) {
            this.$refs[formName].resetFields();
        },

        EditUsersProfile() {

            this.$blockUI.Start();
            this.$http.EditUsersProfile(this.ruleForm)
                .then(response => {
                    this.loginDetails.email = this.ruleForm.Email;
                    this.loginDetails.phone = this.ruleForm.Phone;
                    this.loginDetails.loginName = this.ruleForm.LoginName;
                    this.loginDetails.name = this.ruleForm.Name;
                    localStorage.setItem('currentUser-client', JSON.stringify(this.loginDetails));
                    this.$blockUI.Stop();
                    Swal.fire({
                        icon: 'success',
                        title: '..نجـاح العملية',
                        // text: '<strong>Something went wrong!</strong>',
                        html:
                            response.data,
                        // showCloseButton: true,
                        showCancelButton: false,
                        //confirmButtonText: `حـفظ`,
                        //denyButtonText: `مواق`,
                    }).then(() => {

                    });
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$notify({
                        title: 'خطأ بعملية الاضافة',
                        dangerouslyUseHTMLString: true,
                        type: 'error',
                        message: err.response.data
                    });
                });
        },


        validPassword: function (NewPassword) {

            var PasswordT = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]){8,}.*$/;

            return PasswordT.test(NewPassword);
        },

        submitFormChangePasswoerd(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ChangePassowerd(formName);
                } else {
                    return false;
                }
            });
        },

        ChangePassowerd(formeName) {


            if (this.ChangePasswordruleForm.NewPassword != this.ChangePasswordruleForm.ConfimPassword) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء التأكد من تطابق الرقم السري'
                });
                return;
            }
            if (this.ChangePasswordruleForm.NewPassword.length <= 6) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال الرقم السري تحتوي علي سته ارقام '
                });
                return;
            }
            if (!this.validPassword(this.ChangePasswordruleForm.NewPassword)) {
                this.$message({
                    type: 'error',
                    message: 'عـفوا : يجب ان يحتوي الرقم السري علي حروف صغيرة وكبيرة وارقام'
                });
                return;
            }






            this.$blockUI.Start();
            this.$http.ChangePassword(this.ChangePasswordruleForm)
                .then(response => {
                    this.resetForm(formeName)
                    this.$blockUI.Stop();
                    Swal.fire({
                        icon: 'success',
                        title: '..نجـاح العملية',
                        html:
                            response.data,
                        showCancelButton: false,
                    }).then(() => {

                    });
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$notify({
                        title: 'خطأ بعملية التعديل',
                        dangerouslyUseHTMLString: true,
                        type: 'error',
                        message: err.response.data
                    });
                });
        }


    }
}
