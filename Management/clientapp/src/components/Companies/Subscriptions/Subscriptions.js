////import Swal from 'sweetalert2'
import moment from 'moment';
import HelperMixin from '../../../Shared/HelperMixin.vue'

export default {
    name: 'AddUser',
    mixins: [HelperMixin],
    created() {
        this.GetInfo();
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
        return {
            Info: [],
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            state: 0,
            Total: 0,
            //SelectedDate: new Date(),
        };
    },
    methods: {

        convertDateOnly(date) {
            if (date === null) {
                return "null";
            }
            return moment(date).format('YYYY-MM-DD');
            //return moment(date).format('MMMM Do YYYY');
        },

        GetInfo(pageNo) {
            this.Info = [];
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.SelectedDate = this.convertDateOnly(this.SelectedDate);
            this.$blockUI.Start();
            this.$http.GetSubscriptions(this.pageNo, this.pageSize)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;
                    this.pages = response.data.count;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                    this.pages = 0;
                });
        },





        //DeactivateUser(Id) {
        //    Swal.fire({
        //        title: 'هـل انت متأكد من ايقاف تفعيل المستخدم ؟',
        //        showDenyButton: true,
        //        showCancelButton: false,
        //        confirmButtonText: `تأكيد العملية`,
        //        denyButtonText: `الغاء العملية`,
        //    }).then((result) => {
        //        if (result.isConfirmed) {
        //            this.$blockUI.Start();
        //            this.$http.ChangeStatusUser(Id)
        //                .then(response => {
        //                    this.$blockUI.Stop();
        //                    this.FormPorgress = 100;
        //                    Swal.fire({
        //                        icon: 'success',
        //                        title: '..نجـاح العملية',
        //                        html:
        //                            response.data,
        //                        showCancelButton: true,
        //                        //confirmButtonText: `حـفظ`,
        //                        denyButtonText: `خروج`,
        //                    }).then(() => {

        //                    });
        //                    this.$blockUI.Stop();

        //                    if (this.users.lenght === 1) {
        //                        this.pageNo--;
        //                        if (this.pageNo <= 0) {
        //                            this.pageNo = 1;
        //                        }
        //                    }

        //                    this.GetInfo();


        //                })
        //                .catch((err) => {
        //                    this.$blockUI.Stop();
        //                    this.$notify({
        //                        title: 'خطأ بعملية الايقاف',
        //                        dangerouslyUseHTMLString: true,
        //                        type: 'error',
        //                        message: err.response.data
        //                    });
        //                });
        //            return;
        //        }
        //    })
        //},


        //ActivateUser(Id) {
        //    Swal.fire({
        //        title: 'هـل انت متأكد من  تفعيل المستخدم ؟',
        //        showDenyButton: true,
        //        showCancelButton: false,
        //        confirmButtonText: `تأكيد العملية`,
        //        denyButtonText: `الغاء العملية`,
        //    }).then((result) => {
        //        if (result.isConfirmed) {
        //            this.$blockUI.Start();
        //            this.$http.ChangeStatusUser(Id)
        //                .then(response => {
        //                    this.$blockUI.Stop();
        //                    this.FormPorgress = 100;
        //                    Swal.fire({
        //                        icon: 'success',
        //                        title: '..نجـاح العملية',
        //                        html:
        //                            response.data,
        //                        showCancelButton: true,
        //                        //confirmButtonText: `حـفظ`,
        //                        denyButtonText: `خروج`,
        //                    }).then(() => {

        //                    });
        //                    this.$blockUI.Stop();

        //                    if (this.users.lenght === 1) {
        //                        this.pageNo--;
        //                        if (this.pageNo <= 0) {
        //                            this.pageNo = 1;
        //                        }
        //                    }

        //                    this.GetInfo();


        //                })
        //                .catch((err) => {
        //                    this.$blockUI.Stop();
        //                    this.$notify({
        //                        title: 'خطأ بعملية التفعيل',
        //                        dangerouslyUseHTMLString: true,
        //                        type: 'error',
        //                        message: err.response.data
        //                    });
        //                });
        //            return;
        //        }
        //    })
        //},


        //delteUser(Id) {
        //    Swal.fire({
        //        title: 'هـل انت متأكد من عملية الحذف ؟',
        //        showDenyButton: true,
        //        showCancelButton: false,
        //        confirmButtonText: `تأكيد العملية`,
        //        denyButtonText: `الغاء العملية`,
        //    }).then((result) => {
        //        if (result.isConfirmed) {
        //            this.$blockUI.Start();
        //            this.$http.DeleteUser(Id)
        //                .then(response => {
        //                    this.$blockUI.Stop();
        //                    Swal.fire({
        //                        icon: 'success',
        //                        title: '..نجـاح العملية',
        //                        html:
        //                            response.data,
        //                        showCancelButton: true,
        //                        //confirmButtonText: `حـفظ`,
        //                        denyButtonText: `خروج`,
        //                    }).then(() => {

        //                    });
        //                    this.$blockUI.Stop();
        //                    this.GetInfo();


        //                })
        //                .catch((err) => {
        //                    this.$blockUI.Stop();
        //                    this.$notify({
        //                        title: 'خطأ بعملية الحذف',
        //                        dangerouslyUseHTMLString: true,
        //                        type: 'error',
        //                        message: err.response.data
        //                    });
        //                });
        //            return;
        //        }
        //    })
        //},



        //OpenAddItemDilog() {
        //    this.AddUserDilog = true;
        //},

        //OpenEditItemDilog(item) {
        //    this.ruleForm.Id = item.id;
        //    this.ruleForm.Name = item.name;
        //    this.ruleForm.LoginName = item.loginName;
        //    this.ruleForm.Phone = item.phone;
        //    this.ruleForm.UserType = item.userType;
        //    this.ruleForm.Email = item.email;
        //    this.EditUserDilog = true;
        //},


        //submitForm(formName, type) {
        //    this.$refs[formName].validate((valid) => {
        //        if (valid) {
        //            if (type == 1) {
        //                this.Add(formName);
        //            } else {
        //                this.Edit(formName);
        //            }

        //        } else {
        //            this.$helper.showSwal('warning');
        //            return false;
        //        }
        //    });
        //},

        //resetForm(formName) {
        //    this.$refs[formName].resetFields();
        //},

        //Add(formName) {
        //    this.ruleForm.Id = Number(this.ruleForm.Id);
        //    this.$blockUI.Start();
        //    this.$http.AddUser(this.ruleForm)
        //        .then(response => {
        //            this.$blockUI.Stop();
        //            this.resetForm(formName);
        //            this.GetInfo();
        //            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
        //            this.AddUserDilog = false;
        //        })
        //        .catch((err) => {
        //            this.$blockUI.Stop();
        //            this.$helper.ShowMessage('error', 'خطأ بعملية الاضافة', err.response.data);
        //        });
        //},


        //Edit(formName) {
        //    this.ruleForm.Id = Number(this.ruleForm.Id);
        //    this.$blockUI.Start();
        //    this.$http.EditUser(this.ruleForm)
        //        .then(response => {
        //            this.$blockUI.Stop();
        //            this.resetForm(formName);
        //            this.GetInfo();
        //            this.$helper.ShowMessage('success', 'عملية ناجحة', response.data);
        //            this.EditUserDilog = false;
        //        })
        //        .catch((err) => {
        //            this.$blockUI.Stop();
        //            this.$helper.ShowMessage('error', 'خطأ بعملية الاضافة', err.response.data);
        //        });
        //},



    }
}
