////import Swal from 'sweetalert2';
import moment from 'moment';
import flatPickr from "vue-flatpickr-component";
import HelperMixin from '../../../Shared/HelperMixin.vue'
export default {
    name: 'Courses',
    mixins: [HelperMixin],
    async created() {
        window.scrollTo(0, 0);
        await this.CheckLoginStatus();
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
            CourseId: 0,
            ShowMoreRivew:0,
            //Course: '',

            Course: {
                academicSpecialization: {
                    name: ''
                },
                subject: {
                    name: ''
                }
                ,
                instructor: {
                    name: '',
                    imagePath: '',
                    acountRate: '',
                    courseCount: '',
                    aboutMe: '',
                },
                shapters: [
                    {
                        name: '',
                        number: '',
                        lecture: [
                            {
                                name: '',
                                number: '',
                            }
                        ],
                        exams: [
                            {
                                name: '',
                                descriptions: '',
                                number: '',
                                countQuestions: '',
                            }
                        ]
                    }
                ],

                reviews: [
                    {
                        message: '',
                        rate: '',
                        createdBy: [
                            {
                                imagePath: '',
                                name: '',
                            }
                        ]
                    }
                ],

                reviewsStatisic: [
                    {
                        five: '',
                        foure: '',
                        three: '',
                        two: '',
                        one: '',
                        count: '',
                        sum: '',
                    }
                ],

                moreCourses: [
                    {
                        subject: {
                            subjectId: '',
                            name: '',
                        },
                        academicSpecialization: {
                            academicSpecializationId: '',
                            name: '',
                            academicLevelId: '',
                        },
                        id: '',
                        shapterCount: '',
                        name: '',
                        englishName: '',
                        descriptions: '',
                        teacherName: '',
                        teacherRate: '',
                        rate: '',
                        rateCount: '',
                        isFree: '',
                        price: '',
                        covePhotoPath: '',
                        covePhotoName: '',
                        coverDescriptions: '',
                        taregerLevel: '',
                        levels: '',
                        createdBy: '',
                        createdOn: '',
                        status: '',
                    }
                ],

                id: '',
                name: '',
                englishName: '',
                descriptions: '',
                rate: '',
                isFree: '',
                price: '',
                covePhotoPath: '',
                taregerLevel: '',
                introUrl: '',
                levels: '',
                createdOn: '',
            },

             ruleForm: {
                 CourseId: '',
                 Rate: '',
                 Message: '',
            },
            rules: {
                Rate: this.$helper.Required(),
                Message: this.$helper.Required(),
            },

            
        };
    },
    methods: {

        href(url) {
            this.$router.push(url);
        },

        ChangeReview() {
            if (this.ShowMoreRivew == 0) {
                this.ShowMoreRivew = 1;
            } else {
                this.ShowMoreRivew = 0;
            }
        },



        GetCourseInfo() {
            this.$blockUI.Start();
            this.$http.GetCourseInfo(this.CourseId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Course = response.data.info;
                    this.SetStatiticInfo();
                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },

        SetStatiticInfo() {
            const Five = document.getElementById("Five");
            Five.style.width = this.Course.reviewsStatisic.five+'%';

            const Four = document.getElementById("Four");
            Four.style.width = this.Course.reviewsStatisic.foure + '%';

            const Three = document.getElementById("Three");
            Three.style.width = this.Course.reviewsStatisic.three + '%';

            const Two = document.getElementById("Two");
            Two.style.width = this.Course.reviewsStatisic.two + '%';

            const One = document.getElementById("One");
            One.style.width = this.Course.reviewsStatisic.one + '%';
        },

        OpenCourse(id) {
            window.location.href = "/CourseInfo?id=" + id;
        },


        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {

                    if (!this.ruleForm.Rate || this.ruleForm.Rate <= 0) {
                        this.$helper.showSwal('warning');
                    } else {
                        this.ruleForm.CourseId = Number(this.CourseId);
                        this.$blockUI.Start();
                        this.$http.Rate(this.ruleForm)
                            .then(response => {
                                this.$blockUI.Stop();
                                this.resetForm(formName);
                                this.GetCourseInfo();
                                this.$helper.ShowMessage('success', ' تمت العملية بنجاح', response.data);
                                //setTimeout(() => window.location.href = "/CommingSoon", 1000);
                            })
                            .catch((err) => {
                                this.$blockUI.Stop();
                                this.$helper.ShowMessage('error', 'خطأ بالعملية ', err.response.data);
                            });
                    }

                } else {
                    this.$helper.showSwal('warning');
                    return false;
                }
            });
        },

        resetForm(formName) {
            this.$refs[formName].resetFields();
            this.ruleForm.Rate = 0;
            this.ruleForm.Message = '';
        },


        RegesterToCourse(Id) {
            this.$blockUI.Start();
            this.$http.RequestRegester(Id)
                .then(response => {
                    this.$blockUI.Stop();
                    this.$helper.ShowMessage('success', ' تمت العملية بنجاح', response.data);
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$helper.ShowMessage('error', 'خطأ بالعملية ', err.response.data);
                });
        }


    }
}
