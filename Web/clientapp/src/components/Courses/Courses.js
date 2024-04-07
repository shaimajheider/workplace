////import Swal from 'sweetalert2';
import moment from 'moment';
import flatPickr from "vue-flatpickr-component";
import HelperMixin from '../../Shared/HelperMixin.vue'

export default {
    name: 'Courses',
    mixins: [HelperMixin],
    async created() {
        window.scrollTo(0, 0);
        this.GetInfo();

        if (this.$route.query.AcademicLevelId > 0) {
            this.AcademicLevelId = Number(this.$route.query.AcademicLevelId);
        }

        if (this.$route.query.id > 0) {
            this.AcademicSpecializationId = Number(this.$route.query.id);
        }

        
        
        this.GetAcademicSpecializationNameFirst();
        await this.GetAcademicLevels();
        await this.GetInstructors();
        await this.GetSupjects();
        this.AcademicSpecializationId

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
            pageSize: 6, 
            pages: 0,
            state: 0,
            Info: [],

            AcademicLevelId:'',
            AcademicSpecializationId:'',
            InstructorId:'',
            SubjectId: '',
            Type: '',

            
        };
    },
    methods: {

        href(url) {
            this.$router.push(url);
        },

        GetInfo(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetCoursesInfo(this.pageNo, this.pageSize, this.AcademicLevelId, this.AcademicSpecializationId, this.InstructorId, this.SubjectId, this.Type)
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

        async GetAcademicSpecializationName() {
            this.GetInfo();
            this.AcademicSpecializationId = '',
                await this.GetAcademicSpecialization(this.AcademicLevelId)
        },

        async GetAcademicSpecializationNameFirst() {
            this.GetInfo();
            await this.GetAcademicSpecialization(this.AcademicLevelId)
        },

        OpenCourse(id) {
            window.location.href = "/CourseInfo?id=" + id;
        },



    }
}
