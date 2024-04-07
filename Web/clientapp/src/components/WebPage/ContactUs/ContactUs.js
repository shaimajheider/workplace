export default {
    name: 'home',
    components: {
      
    },
    created() {   
        window.scrollTo(0, 0);
    },
    data() {
        return {
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

        href(url) {
            this.$router.push(url);
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

    }    
}
