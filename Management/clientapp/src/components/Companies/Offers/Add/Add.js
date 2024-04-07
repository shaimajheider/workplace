//import Swal from "sweetalert2";
import moment from "moment";
import HelperMixin from '../../../../Shared/HelperMixin.vue'


export default {
    name: "Add",
    mixins: [HelperMixin],
    async created() {
        await this.CheckLoginStatus();
        await this.GetCompaniesRooms();

        
    },
    
    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
             //return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format("MMMM Do YYYY");
        },
    },
    data() {
        return {


            

            dialogImageUrl: '',
            dialogVisible: false,
            disabled: false,


            ruleForm: {
                Id: 0,

                CompaniesRoomId: '',
                Name: '',
                Descriptions: '',
                Target: '',
                LenthType: '',
                MaxLenth: '',
                LessLenth: '',
                Price: '',
                InitialPaymentPrice: ''
            },



            rules: {
                
                CompaniesRoomId: this.$helper.RequiredInput('الغرفة '),
                Name: this.$helper.RequiredInput('الاسم '),
                Descriptions: this.$helper.RequiredInput('الوصف '),
                Target: this.$helper.RequiredInput('الفئة المستهدفة '),
                LenthType: this.$helper.RequiredInput('النوع '),
                MaxLenth: this.$helper.RequiredInput('اقصي قيمة  '),
                LessLenth: this.$helper.RequiredInput('اقصي قيمة  '),
                Price: this.$helper.RequiredInput('السعر '),
                InitialPaymentPrice: this.$helper.RequiredInput('سعر الحجز '),

            },
        };
    },
    
    methods: {


        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(this.ruleForm.Id);
                    this.ruleForm.FloorCount = Number(this.ruleForm.FloorCount);
                    this.ruleForm.ClassRoomCount = Number(this.ruleForm.ClassRoomCount);
                    this.ruleForm.MaxLenth = Number(this.ruleForm.MaxLenth);
                    this.ruleForm.LessLenth = Number(this.ruleForm.LessLenth);
                    this.ruleForm.Price = Number(this.ruleForm.Price);
                    this.ruleForm.InitialPaymentPrice = Number(this.ruleForm.InitialPaymentPrice);

                    this.$blockUI.Start();
                    this.$http.AddOffers(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.resetForm(formName);
                            this.RemoveAllAttachment();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('error', 'خطأ بعملية الاضافة', err.response.data);
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













    },
};
