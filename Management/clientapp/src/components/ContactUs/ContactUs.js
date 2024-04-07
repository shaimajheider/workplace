////import Swal from 'sweetalert2'
import moment from 'moment';
import HelperMixin from '../../Shared/HelperMixin.vue'

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
            this.$http.GetContactUs(this.pageNo, this.pageSize, /*this.SelectedDate *//*, this.UserType, this.MunicipalitId, this.HospitalsId*/)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;
                    this.pages = response.data.count;
                    //this.Total = response.data.total;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                    this.pages = 0;
                });
        },




//,



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
