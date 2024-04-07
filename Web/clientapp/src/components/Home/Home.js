import HelperMixin from '../../Shared/HelperMixin.vue'
//import CryptoJS from 'crypto-js';
export default {
    name: 'home',
    mixins: [HelperMixin],
    components: {
       
    },
    async created() {
        window.scrollTo(0, 0);
   
    },
    data() {
        return {
            AcademicSpecializationInfo:[],
            Courses: [],
            pageNo: 1,
            pageSize: 4,

            ruleForm: {
                Name: '',
                Email: '',
                Phone: '',
                Mesaage: '',
            },
            rules: {
                Name: this.$helper.DynamicArabicEnterRequired('الاسم '),
                Mesaage: this.$helper.Required('الرسالة '),
                Phone: [
                    {
                        required: true,
                        message: "الرجاء إدخال رقم الهاتف",
                        trigger: "blur",
                    },
                    {
                        min: 9,
                        max: 13,
                        message: "يجب ان يكون طول رقم الهاتف 9 ارقام علي الاقل",
                        trigger: "blur",
                    },
                    { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' }
                ],
                Email: [
                    { required: true, message: 'الرجاء إدخال البريد الإلكتروني', trigger: 'blur' },
                    { required: true, pattern: /\S+@\S+\.\S+/, message: 'الرجاء إدخال البريد الإلكتروني بطريقه صحيحه', trigger: 'blur' }
                ],
            },

        };
    },
    methods: {


        GetAcademicSpecializationInfo() {
            this.$blockUI.Start();
            this.$http.GetAcademicSpecializationInfo()
                .then(response => {
                    this.$blockUI.Stop();
                    this.AcademicSpecializationInfo = response.data.info;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },

        GetCourses(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetCoursesInfoOrderByTop(this.pageNo, this.pageSize)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Courses = response.data.info;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },

        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.$blockUI.Start();
                    this.$http.CountactUs(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.resetForm(formName);
                            this.$helper.ShowMessage('success', ' تمت العملية بنجاح', response.data);
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('error', 'خطأ بعملية الارسال', err.response.data);
                        });

                } else {
                    this.$helper.showSwal('warning');
                    return false;
                }
            });
        },

        resetForm(formName) {
            this.$refs[formName].resetFields();
        },


        href(url) {
            this.$router.push(url);
        },

        herfParm(name,id) {
            window.location.href = name+"?id=" + id;
        },

        //hrefPar(url, id) {
        //    if (id <= 0) {
        //        this.$router.push(url);
        //    } else {
        //        debugger
        //        var en = this.encrypt('id=');
        //        this.$router.push(url+'?'+en);
        //    }
            
        //},

        //encrypt: function encrypt(data, SECRET_KEY) {
        //    var dataSet = CryptoJS.AES.encrypt(JSON.stringify(data), SECRET_KEY);
        //    dataSet = dataSet.toString();
        //    return dataSet;
        //},
        //decrypt: function decrypt(data, SECRET_KEY) {
        //    data = CryptoJS.AES.decrypt(data, SECRET_KEY);
        //    data = JSON.parse(data.toString(CryptoJS.enc.Utf8));
        //    return data;
        //},



    }    
}
