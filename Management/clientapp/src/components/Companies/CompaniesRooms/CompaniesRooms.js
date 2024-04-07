import Swal from "sweetalert2";
import moment from "moment";
import HelperMixin from '../../../Shared/HelperMixin.vue'
export default {
    name: "AddUser",
    mixins: [HelperMixin],
    async created() {
        await this.CheckLoginStatus();
        this.GetInfo();








        var today = new Date();
        var dd = String(today.getDate()).padStart(2, "0");
        var mm = String(today.getMonth() + 1).padStart(2, "0"); //January is 0!
        var yyyy = today.getFullYear();
        this.dateNow = mm + "/" + dd + "/" + yyyy;


    },
    components: {
    },
    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
            return moment(date).format("MMMM Do YYYY");
        },

        momentTime: function (date) {
            if (date === null) {
                return "لا يوجد";
            }
            return moment(date).format("Do/MM/YYYY || HH:mm");
        },

    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            state: 0,

            Info: [],
            Messages: [],
            Offices: [],

            ByDate: '',
            OfficeId: '',
            MessageId: '',

            SelectedItem: '',

            AddViewDilogRequest: false,

            dialogVisible: false,
            dialogImageUrl: '',


        };
    },
    methods: {

        GetInfo(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }

            if (this.ByDate)
                this.ByDate = this.ChangeDate(this.ByDate);

            this.$blockUI.Start();
            this.$http
                .GetCompaniesRooms(this.pageNo, this.pageSize)
                .then((response) => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;
                    this.pages = response.data.count;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },

        Refresh() {
            this.OfficeId = '';
            this.ByDate = '';
            this.MessageId = '';
            this.SelectedItem = '';
            this.GetInfo();
        },

        viewInfo(item) {
            this.SelectedItem = item;
            this.AddViewDilogRequest = true;
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

        handleRemove(Id) {
            Swal.fire({
                title: "هـل انت متأكد من عملية حذف المرفق ؟",
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: `تأكيد العملية`,
                denyButtonText: `الغاء العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.DeleteMessageAttachment(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetInfo();
                            this.AddViewDilogRequest = false;
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('error', 'خطأ بعملية الحذف', err.response.data);
                        });
                    return;
                }
            });
        },

        Delete(Id) {
            Swal.fire({
                title: "هـل انت متأكد من عملية الحذف ؟",
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: `تأكيد العملية`,
                denyButtonText: `الغاء العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.DeleteCompaniesRooms(Id)
                        .then((response) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.GetInfo();
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('error', 'خطأ بعملية الحذف', err.response.data);
                        });
                    return;
                }
            });
        },











        ChangeDate(date) {
            if (date === null) {
                return "فارغ";
            }
            return moment(date).format("YYYY-MM-DD");
        },

    },
};
