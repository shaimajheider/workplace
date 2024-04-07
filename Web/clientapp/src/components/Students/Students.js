import Swal from 'sweetalert2';
import moment from 'moment';
import HelperMixin from '../../Shared/HelperMixin.vue'
export default {
    name: 'Student',
    mixins: [HelperMixin],
    async created() {
        await this.CheckLoginStatusRequierd();
        this.GetStudentInfo();
        this.state = this.$route.query.id;
        this.ruleForm.LoginName = this.loginDetails.loginName;
        this.ruleForm.FirstName = this.loginDetails.firstName;
        this.ruleForm.FatherName = this.loginDetails.fatherName;
        this.ruleForm.SirName = this.loginDetails.sirName;
        this.ruleForm.Email = this.loginDetails.email;
        this.ruleForm.Phone = this.loginDetails.phone;
        this.ruleForm.AboutMe = this.loginDetails.aboutMe;
    },
    components: {
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
    data() {
        var validatePass = (rules, value, callback) => {
            if (value === '') {
                callback(new Error('الرجاء إدخال كلمة المرور'));
            } else {
                if (this.ruleForm.ConfimPassword !== '') {
                    this.$refs.ruleForm.validateField('ConfimPassword');
                }
                callback();89+7
            }
        };
        var validatePass2 = (rrulesule, value, callback) => {
            if (value === '') {
                callback(new Error('الرجاء كتابه اعاده كلمه المرور'));
            } else if (value !== this.ruleForm.NewPassword) {
                callback(new Error('الرجاء التأكد من تطابق كلمة المرور'));
            } else {
                callback();
            }
        };
        return {
            pageNo: 1,
            pageSize: 10, 
            pages: 0,
            state: 0,
            charge: 0,
            Info: [],

            VoicherCardNumber: '',

            ruleForm: {
                FirstName: '',
                FatherName: '',
                SirName: '',
                LoginName: '',
                Email: '',
                ImagePath: '',
                ImageName: '',
                fileBase64: '',
                Phone: '',
                CollageId: '',
                Password: '',
                NewPassword: '',
                ConfirmPassword: '',
                AboutMe: '',

            },
            rules: {
                FirstName: this.$helper.DynamicArabicEnterRequired('الاسم '),
                FatherName: this.$helper.DynamicArabicEnterRequired('اسم الاب '),
                SirName: this.$helper.Required('اللقب '),
                LoginName: this.$helper.Required('اسم الدخول '),
                Email: [
                    { required: true, message: 'الرجاء إدخال البريد الإلكتروني', trigger: 'blur' },
                    { required: true, pattern: /\S+@\S+\.\S+/, message: 'الرجاء إدخال البريد الإلكتروني بطريقه صحيحه', trigger: 'blur' }
                ],
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
                CollageId: [
                    {
                        required: true,
                        message: "الرجاء إدخال رقـم القيد",
                        trigger: "blur",
                    },
                    {
                        min: 2,
                        max: 20,
                        message: "يجب ان يكون طول رقم القيد من 2 الي 20 الحرف",
                        trigger: "blur",
                    },
                    { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' }
                ],
                PasswordPassword: [
                    { validator: validatePass, trigger: 'blur' },
                    { required: true, pattern: /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]){8,}.*$/, message: '  تتكون كلمة المرور من حروف صغيرة وكبيرو وأرقام', trigger: 'blur' }
                ],
                ConfirmPassword: [
                    { validator: validatePass2, trigger: 'blur' },
                    { required: true, pattern: /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]){8,}.*$/, message: ' تتكون كلمة المرور من حروف صغيرة وكبيرو وأرقام', trigger: 'blur' }
                ],
                NewPassword: [
                    { validator: validatePass, trigger: 'blur' },
                    { required: true, pattern: /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]){8,}.*$/, message: ' تتكون كلمة المرور من حروف صغيرة وكبيرو وأرقام', trigger: 'blur' }
                ],
                AboutMe: this.$helper.Required('النبذة الشخصية  '),
            },

        };
    },
    methods: {

        href(url) {
            this.$router.push(url);
        },

        GetStudentInfo() {
            this.$blockUI.Start();
            this.$http.GetStudentFullInfo()
                .then(response => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;
                    this.SetStatiticInfo();
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    if (err.response.status == 401) {
                        this.$helper.ShowMessage('error', 'الرجاء التأكد من تسجيل الدخول ', err.response.data);
                        this.logout();
                    } else if (err.response.status == 400) {
                        this.$helper.ShowMessage('error', 'الرجاء التأكد من تسجيل الدخول ', err.response.data);
                    } else {
                        window.location.href = "/";
                    }
                    
                });
        },

        IsLogin() {
            this.$http.IsLoggedin()
                .then(response => {
                    if (!response.data) {
                        this.logout();
                    }
                    
                })
        },

        SetStatiticInfo() {
            if (this.Info.studentCourseInfo.length > 0) {

                this.Info.studentCourseInfo.forEach((element) => {
                    if (element.shapterCount > 0) {
                        element.shapterPresntage = (element.shapterPresntage / element.shapterCount) * 100;
                    } else {
                        element.shapterPresntage = 0;
                    }
                });

            }

            
        },

        SelectAttachment(file) {
            let str = file.raw.type;
            str = str.substring(0, 5);

            if (str != "image") {
                this.$helper.ShowMessage('error', 'خطأ بالعملية', 'الرجاء التأكد من نوع الملف');
            }

            var $this = this;
            var reader = new FileReader();
            reader.readAsDataURL(file.raw);
            reader.onload = function () {
                $this.ruleForm.ImageName = file.raw.name;
                $this.ruleForm.fileBase64 = reader.result;
            };


            this.$http.ChangeProfilePicture(this.ruleForm)
                .then(response => {
                    this.$blockUI.Stop();
                    this.$helper.ShowMessage('success', 'عملية ناجحة', response.data.message);
                    this.loginDetails.imagePath = response.data.imagePath;
                    this.localStorage.removeItem('currentUser-client');
                    localStorage.setItem('currentUser-client', this.encrypt(JSON.stringify(response.data), this.PlatFormPass));
                    //setTimeout(() => this.logout(), 1000);

                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$helper.ShowMessage('error', 'خطأ بالعملية', err.response.data);
                });

        },  

        submitPersonalInfoForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.$blockUI.Start();
                    this.$http.EditPersonalInfo(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            //setTimeout(() => window.location.href = "/Login", 1000);
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('error', 'خطأ بالعملية', err.response.data);
                        });


                } else {
                    this.$helper.showSwal('warning');
                    return false;
                }
            });
        },

        submitPasswordForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {

                    if (this.ruleForm.NewPassword != this.ruleForm.ConfirmPassword)
                        this.$helper.ShowMessage('error', 'خطأ بالعملية', 'الرجاء التأكد من تطابق كلمات المرور');

                    this.$blockUI.Start();
                    this.$http.ChangePassword(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.ruleForm.Password = '';
                            this.ruleForm.NewPassword = '';
                            this.ruleForm.ConfimPassword = '';
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('error', 'خطأ بالعملية', err.response.data);
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

        ChangeDeviceRequest() {
            Swal.fire({
                title: 'هل أنت متأكد من تقديم طلب تغير الجهاز  ؟',
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: `تأكيد العملية`,
                denyButtonText: `الغاء العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.ChangeDeviceRequest()
                        .then(response => {

                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'تمت العملية بنجاح', response.data);

                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            if (err.response.status == 401) {
                                this.$helper.ShowMessage('error', 'الرجاء التأكد من تسجيل الدخول ', err.response.data);
                                this.logout();
                            } else if (err.response.status == 400) {
                                this.$helper.ShowMessage('error', 'حدت خطاء في العملية  ', err.response.data);
                            } else {
                                window.location.href = "/";
                            }

                        });
                    return;

                }
            })
        },

    }
}
