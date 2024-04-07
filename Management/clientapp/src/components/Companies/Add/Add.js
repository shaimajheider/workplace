//import Swal from "sweetalert2";
import moment from "moment";
import HelperMixin from '../../../Shared/HelperMixin.vue'


export default {
    name: "Add",
    mixins: [HelperMixin],
    async created() {
        await this.CheckLoginStatus();
        await this.GetPorts();
        await this.GetAgencies();
        await this.GetArrestedCases();
        await this.GetCities();

        
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

                Name: '',
                LoginName: '',
                Email: '',
                Phone: '',

                OwnerName: '',
                OwnerPhone: '',

                LocationDescriptions: '',
                LocationLink: '',

                FloorCount: 0,
                ClassRoomCount: 0,
                MeetingRoomCount: 0,
                TraningRoomCount: 0,

                PrivateRoomCount: 0,
                OfficeCount: 0,

                Notes: '',

                Attachments: [],
            },



            rules: {
                
                Name: this.$helper.DynamicArabicEnterRequired('الاسم رباعي'),
                LoginName: this.$helper.LoginName(),
                Email: this.$helper.Email(),
                Phone: this.$helper.Phone(),

                OwnerName: this.$helper.DynamicArabicEnterRequired(' إسم المدير العام '),
                OwnerPhone: this.$helper.Phone(),

                LocationDescriptions: this.$helper.RequiredInput('   وصف الموقع  '),
                LocationLink: this.$helper.RequiredInput('   رابط للموقع   '),

                FloorCount: this.$helper.RequiredInput('    عدد الطوابق   '),
                ClassRoomCount: this.$helper.RequiredInput('    عدد القاعات   '),
                MeetingRoomCount: this.$helper.RequiredInput('    عدد غرف الاجتماع   '),
                TraningRoomCount: this.$helper.RequiredInput('    عدد غرف التدريب   '),

                PrivateRoomCount: this.$helper.RequiredInput('    عدد القاعات الخاصة    '),
                OfficeCount: this.$helper.RequiredInput('    عدد  المكاتب    '),

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
                    this.ruleForm.MeetingRoomCount = Number(this.ruleForm.MeetingRoomCount);
                    this.ruleForm.TraningRoomCount = Number(this.ruleForm.TraningRoomCount);
                    this.ruleForm.PrivateRoomCount = Number(this.ruleForm.PrivateRoomCount);
                    this.ruleForm.OfficeCount = Number(this.ruleForm.OfficeCount);

                    this.$blockUI.Start();
                    this.$http.AddCompanies(this.ruleForm)
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
