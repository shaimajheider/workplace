import HelperMixin from '../../../Shared/HelperMixin.vue'
export default {
    name: 'Sinup',
    mixins: [HelperMixin],
    components: {
    },
     async created() {
        window.scrollTo(0, 0);
         await this.GetCities();
    },
    data() {
        
        return {

            dialogImageUrl: '',
            dialogVisible: false,
            disabled: false,

            ruleForm: {
                
                ProgramId: '',
                CityId: '',
                FacilityId: '',
                FirstName: '',
                FatherName: '',
                GrandFatherName: '',
                SirName: '',
                Nid: '',
                Phone: '',
                BirthDate: '',
                Gender: '',
                DoctorName: '',

                Attachments: [],
            },
            rules: {
                ProgramId: this.$helper.Required('البرنامج المستهدف '),
                CityId: this.$helper.Required('المدينة  '),
                FacilityId: this.$helper.Required('المرفق الطبي  '),
                FirstName: this.$helper.DynamicArabicEnterRequired('الاسم '),
                FatherName: this.$helper.DynamicArabicEnterRequired('اسم الاب '),
                GrandFatherName: this.$helper.DynamicArabicEnterRequired('اسم الجد '),
                SirName: this.$helper.DynamicArabicEnterRequired('اللقب '),
                DoctorName: this.$helper.DynamicArabicEnterRequired('اسم الطبيب المعالج '),
                Nid: this.$helper.NID(),
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
                BirthDate: this.$helper.Required('تاريخ الميلاد '),
                Gender: this.$helper.Required('الجنس '),
            },


        };
    },
    methods: {

        href(url) {
            this.$router.push(url);
        },

        async GetFacilitiesInfo() {
            this.ruleForm.FacilityId = '',
                await this.GetFacilities(this.ruleForm.CityId)
        },

        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {

                    this.$blockUI.Start();
                    this.$http.RegesterApp(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.resetForm(formName);
                            this.$helper.ShowMessage('success', ' تمت عملية التسجيل بنجاح', response.data);
                            setTimeout(() => window.location.href = "/CommingSoon", 1000);
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('error', 'خطأ بعملية التسجيل ', err.response.data);
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





        SelectCoverAttachment(file) {

            let str = file.raw.type;
            str = str.substring(0, 5);


            if (str != "image" && str != "appli" && str != "text/" && str != "video") {
                this.$helper.ShowMessage('error', 'خطأ بالعملية', 'الرجاء التأكد من نوع الملف');
            }

            if (str != "image")
                file.url = this.ServerUrl + '/assets/img/small-logos/file.png';


            //var reader1 = new FileReader();
            //reader.readAsDataURL(file.raw);

            var $this = this;
            var reader = new FileReader();
            reader.readAsDataURL(file.raw);
            reader.onload = function () {

                if ($this.ruleForm && $this.ruleForm.Attachments.length !== 0) {
                    if (!$this.ruleForm.Attachments.some(item => item.ImageName === file.raw.name)) {
                        const obj = {
                            ImageName: file.raw.name,
                            ImageType: file.raw.type,
                            Url: file.url,
                            fileBase64: reader.result,
                        }
                        $this.ruleForm.Attachments.push(obj);
                    } else {
                        $this.$helper.ShowMessage('error', 'خطأ بالعملية', 'اسم الملف موجود مسبقا');
                        const imageUrl = file.url;
                        const imageElement = document.querySelector(`img[src="${imageUrl}"]`);
                        const parentElement = imageElement.parentNode;
                        const grandparentElement = parentElement.parentNode;

                        if (imageElement) {
                            grandparentElement.remove();
                        }
                    }
                } else {
                    const obj = {
                        ImageName: file.raw.name,
                        Url: file.url,
                        ImageType: file.raw.type,
                        fileBase64: reader.result,
                    }
                    $this.ruleForm.Attachments.push(obj);
                }

            };

        },

        handleRemove(file) {

            if (this.ruleForm && this.ruleForm.Attachments.length !== 0) {

                if (this.ruleForm.Attachments.some(item => item.ImageName === file.name)) {
                    const indexToDelete = this.ruleForm.Attachments.findIndex(item => item.ImageName === file.name);

                    if (indexToDelete !== -1) {
                        this.ruleForm.Attachments.splice(indexToDelete, 1);
                    }
                }
            }

            const imageUrl = file.url;
            const imageElement = document.querySelector(`img[src="${imageUrl}"]`);
            const parentElement = imageElement.parentNode;
            const grandparentElement = parentElement.parentNode;

            if (imageElement) {
                grandparentElement.remove();
            }
        },

        handlePictureCardPreview(file) {
            this.dialogImageUrl = file.url;
            this.dialogVisible = true;
        },

        handleDownload(file) {
            const link = document.createElement('a');
            link.href = file.url;
            link.target = '_blank';
            link.download = '';
            link.click();
        },

        RemoveAllAttachment() {
            const elements = document.querySelectorAll('.el-upload-list--picture-card .el-upload-list__item');
            elements.forEach(element => element.remove());
            this.ruleForm.Attachments = [];
        }






    }
}
