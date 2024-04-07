////import Swal from 'sweetalert2'
import moment from 'moment';
import flatPickr from "vue-flatpickr-component";
import HelperMixin from '../../../Shared/HelperMixin.vue'
export default {

    name: 'Instructors',
    mixins: [HelperMixin],
    async created() {
        window.scrollTo(0, 0);
        this.InstructorId = this.$route.query.id;
        this.GetInfo();
    },
    components: {
        flatPickr,
    },
    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
            return moment(date).format('MMMM Do YYYY');
        }
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 8,
            pages: 0,
            state: 0,
            InstructorId: 0,
            Info: '',
            Courses: [],
        };
    },

    methods: {

        href(url) {
            this.$router.push(url);
        },

        herfParm(name, id) {
            window.location.href = name + "?id=" + id;
        },

        GetInfo(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetInstructorsById(this.InstructorId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Info = response.data.info;
                    this.Courses = response.data.courses;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },


    }
}
