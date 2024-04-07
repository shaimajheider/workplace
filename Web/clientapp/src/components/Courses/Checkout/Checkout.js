import Swal from 'sweetalert2';
import moment from 'moment';
import flatPickr from "vue-flatpickr-component";
import HelperMixin from '../../../Shared/HelperMixin.vue'
export default {
    name: 'Courses',
    mixins: [HelperMixin],
    async created() {
        window.scrollTo(0, 0);
        await this.CheckLoginStatusRequierd();
        this.CourseId = this.$route.query.id;
        this.GetCourseInfo();



    },
    components: {
        flatPickr,
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
            pageNo: 1,
            pageSize: 10, 
            pages: 0,
            state: 0,
            Info: '',
            
            
            
        };
    },
    methods: {

        href(url) {
            this.$router.push(url);
        },


        GetCourseInfo() {
            this.$blockUI.Start();
            this.$http.GetCourseInfoById(this.CourseId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },

        Checkout() {
            var bodyObject = {
                id: Number(this.CourseId)
            }
            Swal.fire({
                title: 'سيتم خصم القيمة من المحفظة هل تريد المتابعة ؟',
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: `تأكيد العملية`,
                denyButtonText: `الغاء العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.$blockUI.Start();
                    this.$http.Checkout(bodyObject)
                        .then(response => {
                          
                            this.$blockUI.Stop();
                            this.$helper.ShowMessage('success', 'تهانينا تمت العملية بنجاح', response.data);
                            this.href('/Students' + '?' + 'id=' + 2);

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
