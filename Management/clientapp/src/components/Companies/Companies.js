import Swal from "sweetalert2";
import moment from "moment";
import HelperMixin from '../../Shared/HelperMixin.vue';

export default {
    name: "Transfer",
    mixins: [HelperMixin],
    async created() {
        await this.CheckLoginStatus();
        await this.GetPaymentMethods();

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
            // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format("MMMM Do YYYY");
        },

        momentTime: function (date) {
            if (date === null) {
                return "فارغ";
            }
            // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format('DD/MM/YYYY || h:mm a');
        },
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            state: 0,
            Info: [],
            Wallet: [],
            WalletPurchases: [],


            PortId: '',
            UserType: '',


            AddDialog: false,
            EditDialog: false,
            InfoDialog: false,

            RoomDialog: false,
            WalletDiloag: false,
            AddValueDiloag: false,

            SelectedItem: '',


            ruleForm: {
                Id: '',
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




            WalletruleForm: {
                Id: 0,
                CompaniesId: '',
                PaymentMethodId: '',
                Value: '',
                ProcessType: 1,
            },
            Walletrules: {
                PaymentMethodId: this.$helper.Required(),
                Value: this.$helper.NumberOnly(),
            },


            
        };
    },
    methods: {

        GetInfo(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }

            this.$blockUI.Start();
            this.$http
                .GetCompanies(this.pageNo, this.pageSize,2, this.UserType, this.PortId)
                .then((response) => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;
                    this.pages = response.data.count;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                    this.pages = 0;
                });
        },

        Delete(Id) {
            Swal.fire({
                title: "هـل انت متأكد من عملية الحذف  ؟",
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: `تأكيد العملية`,
                denyButtonText: `الغاء العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.DeleteCompanies(Id)
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

        ResetPassword(Id) {
            Swal.fire({
                title: 'هـل انت متأكد من تهيئة كلمة المرور ؟',
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: `تأكيد العملية`,
                denyButtonText: `الغاء العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.RestePasswordCompanies(Id)
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

        DeactivateUser(Id) {
            Swal.fire({
                title: 'هـل انت متأكد من ايقاف تفعيل حساب الشركة  ؟',
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: `تأكيد العملية`,
                denyButtonText: `الغاء العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.ChangeStatusCompanies(Id)
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

        ActivateUser(Id) {
            Swal.fire({
                title: 'هـل انت متأكد من  تفعيل حساب الشركة  ؟',
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: `تأكيد العملية`,
                denyButtonText: `الغاء العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.ChangeStatusCompanies(Id)
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

        OpentEditDialog(item) {
            this.SelectedItem = item;
            this.ruleForm.Id = item.id;
            this.ruleForm.Name = item.name;
            this.ruleForm.LoginName = item.loginName;
            this.ruleForm.Phone = '0' + item.phone;
            this.ruleForm.Email = item.email;
            this.ruleForm.OwnerName = item.ownerName;
            this.ruleForm.OwnerPhone = item.ownerPhone;
            this.ruleForm.LocationDescriptions = item.locationDescriptions;
            this.ruleForm.LocationLink = item.locationLink;
            this.ruleForm.FloorCount = item.floorCount;
            this.ruleForm.ClassRoomCount = item.classRoomCount;
            this.ruleForm.MeetingRoomCount = item.meetingRoomCount;
            this.ruleForm.TraningRoomCount = item.traningRoomCount;
            this.ruleForm.PrivateRoomCount = item.privateRoomCount;
            this.ruleForm.OfficeCount = item.officeCount;
            this.EditDialog = true;
        },

        submitEditForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.ruleForm.Id = Number(this.ruleForm.Id);
                    //if (this.ruleForm.UserType != 2) {
                    //    this.ruleForm.KidneyCenterId = Number(0);
                    //}
                    this.$blockUI.Start();
                    this.$http.EditCompanies(this.ruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.resetForm(formName);
                            this.$helper.ShowMessage('success', ' تمت عملية التعديل بنجاح', response.data);
                            this.GetInfo();
                            this.EditDialog = false;
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('error', 'خطأ بعملية التعديل', err.response.data);
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




        //RoomDialog
        ViewRoomDilaog(item) {
            this.SelectedItem = item;
            this.RoomDialog = true;

        },




        //WalletDiloag
        ViewWalletDiloag(item) {
            this.SelectedItem = item;
            this.state = 1;
            this.GetWalletInfo(item.id);

        },

        GetWalletInfo(Id) {
            this.$blockUI.Start();
            this.$http
                .GetCompaniesWalletInfo(Id)
                .then((response) => {
                    this.$blockUI.Stop();
                    this.Wallet = response.data.info;
                    this.WalletPurchases = response.data.walletPurchases;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },

        OpenAddValueDiloag() {
            this.AddValueDiloag = true;
            this.WalletruleForm.Value = '';
            this.WalletruleForm.PaymentMethodId = '';
        },

        submitWalletruleForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.WalletruleForm.Id = Number(this.WalletruleForm.Id);
                    this.WalletruleForm.CompaniesId = Number(this.SelectedItem.id);
                    this.WalletruleForm.Value = Number(this.WalletruleForm.Value);
                    this.WalletruleForm.ProcessType = Number(this.WalletruleForm.ProcessType);
                    this.$blockUI.Start();
                    this.$http.RechargeWallet(this.WalletruleForm)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.resetForm(formName);
                            this.GetWalletInfo(this.SelectedItem.id);
                            this.GetInfo();
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                            this.AddValueDiloag = false;
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

        DeleteTransacitons(id) {
            Swal.fire({
                title: 'هـل انت متأكد من عملية الحذف ؟',
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: `تأكيد العملية`,
                denyButtonText: `الغاء العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.DeleteWalletTransacitons(id)
                        .then(response => {
                            this.$blockUI.Stop();
                            this.GetInfo();
                            this.GetStudentWalletInfo(this.selectedItem.id);
                            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('error', 'خطأ بعملية الحذف', err.response.data);
                        });
                    return;
                }
            })
        },




        




        Back() {
            this.state = 0;
            this.Refresh();
        },

        Refresh() {
            this.PortId = '';
            this.UserType = '';
            this.GetInfo();
        },

    },
};
