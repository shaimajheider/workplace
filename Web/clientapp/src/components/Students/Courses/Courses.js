import moment from 'moment';
import Swal from 'sweetalert2';
import flatPickr from "vue-flatpickr-component";
import HelperMixin from '../../../Shared/HelperMixin.vue';

export default {
    name: 'StudentCourse',
    mixins: [HelperMixin],
    async created() {
        await this.CheckLoginStatusRequierd();
        this.CourseId = Number(this.$route.query.id);
        this.GetInfo();

        this.isAndroid = this.loginDetails.isAndroid;
        
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
            isAndroid:false,
            status :0,
            blockService :null,
            loading: null,
            Info:[],
            CourseId:'',
            LecturePath:'',
            LectureType:'',
            SelectedPDFPath:'',
            LectureOrExam: 0,
            SelectedExam: '',

            timerValue: 0,
            remainingTime: 60 * 60, // Remaining time in seconds (30 minutes)
            intervalId: null,

            StartExam: false,


            Auth: {
                otp: '',
                playbackInfo: '',
            },
        };
    },

    mounted() {
        this.startTimer();
    },
    computed: {
        formattedTime() {
            const minutes = Math.floor(this.remainingTime / 60);
            const seconds = this.remainingTime % 60;
            return `${this.pad(minutes)}:${this.pad(seconds)}`;
        },
    },
    methods: {

        ChangeMobileSecition() {

            if (document.getElementById("MobileMenuSection").classList.contains('active')) {
                document.getElementById("MobileMenuSection").classList.remove('active');
            } else {
                document.getElementById("MobileMenuSection").classList.add('active');
            }
        },


        Show(id) {
            //var collapse = document.querySelector('#headingTwo_' + id);
            //if (collapse != null) {
            //    collapse.setAttribute("aria-expanded", "true");
            //}

            //for (var i = 0; i < this.Info.length; i++) {
            //    var item = document.getElementById('button_' + this.Info[i].id);
            //    if (item != null) {
            //        var section1 = document.getElementById('#section_' + this.Info[i].id);
            //        if (section1 != null) {
            //            var isShowClassAdded = section1.classList.toggle("show");

            //            if (isShowClassAdded) {
            //                item.classList.remove("collapsed");
            //                item.setAttribute("aria-expanded", "false");
            //            } 
            //        }
                        
            //    }
            //}

            var button = document.getElementById('button_' + id);
            if (button != null) {
                button.classList.add("collapsed");
                button.setAttribute("aria-expanded", "true");
            }
          
                

            var section = document.getElementById('#section_' + id);
            if (section != null) {
                section.classList.toggle("show");

                var isShowClassAdded1 = section.classList.contains("show");

                if (!isShowClassAdded1) {
                    button.classList.remove("collapsed");
                    button.setAttribute("aria-expanded", "false");
                }

            }
                

        },

        Showsub(id) {
            

            var button = document.getElementById('subbutton_' + id);
            if (button != null) {
                button.classList.add("collapsed");
                button.setAttribute("aria-expanded", "true");
            }
          
                

            var section = document.getElementById('#subsection_' + id);
            if (section != null) {
                section.classList.toggle("show");

                var isShowClassAdded1 = section.classList.contains("show");

                if (!isShowClassAdded1) {
                    button.classList.remove("collapsed");
                    button.setAttribute("aria-expanded", "false");
                }

            }
                

        },



        MobileShow(id) {
            var button = document.getElementById('Mobilebutton_' + id);
            if (button != null) {
                button.classList.add("collapsed");
                button.setAttribute("aria-expanded", "true");
            }



            var section = document.getElementById('#Mobilesection_' + id);
            if (section != null) {
                section.classList.toggle("show");

                var isShowClassAdded1 = section.classList.contains("show");

                if (!isShowClassAdded1) {
                    button.classList.remove("collapsed");
                    button.setAttribute("aria-expanded", "false");
                }

            }


        },

        MobileShowsub(id) {


            var button = document.getElementById('Mobilesubbutton_' + id);
            if (button != null) {
                button.classList.add("collapsed");
                button.setAttribute("aria-expanded", "true");
            }



            var section = document.getElementById('#Mobilesubsection_' + id);
            if (section != null) {
                section.classList.toggle("show");

                var isShowClassAdded1 = section.classList.contains("show");

                if (!isShowClassAdded1) {
                    button.classList.remove("collapsed");
                    button.setAttribute("aria-expanded", "false");
                }

            }


        },







        
        Start() {
            this.blockService = this.$loading({
                fullscreen: true,
                text: 'تـحميل البيـانات'
            });
        },
        Stop() {
            this.blockService.close();
        },


        GetInfo() {
            this.Start();
            this.$http.GetCourseLecture(this.CourseId)
                .then(response => {
                    this.Stop();
                    this.Info = response.data.info;
                })
                .catch((err) => {
                    this.Stop();
                    if (err.response.status == 401) {
                        this.$helper.ShowMessage('error', 'الرجاء التأكد من تسجيل الدخول ', err.response.data);
                        this.logout();
                    } else if (err.response.status == 400) {
                        this.$helper.ShowMessage('error', 'لم يتم العتور على الملف', err.response.data);
                    } else {
                        window.location.href = "/";
                    }

                });
        },

        
        OpenLecture(id, Type) {
            this.LecturePath = '';
            if (Type == 1) {
                this.LectureOrExam = Type;
                this.Start();
                this.$http.CourseLectur(id)
                    .then(response => {
                        this.Stop();
                        this.LecturePath = response.data.info;
                        this.LectureType = response.data.type;
                        this.Auth.otp = response.data.vdocipher.otp;
                        this.Auth.playbackInfo = response.data.vdocipher.playbackInfo;
                    })
                    .catch((err) => {
                        this.Stop();
                        if (err.response.status == 401) {
                            this.$helper.ShowMessage('error', 'الرجاء التأكد من تسجيل الدخول ', err.response.data);
                            this.logout();
                        } else if (err.response.status == 400) {
                            this.$helper.ShowMessage('error', 'لم يتم العتور على الملف', err.response.data);
                        } else {
                            this.$helper.ShowMessage('error', 'الرجاء مراجعة الادارة  ', err.response.data);
                            window.location.href = "/";
                        }

                    });
            }
        },

        OpenExam(exams) {
            this.LectureOrExam = 60;
            this.SelectedExam = exams;
        },

        startTimer() {
            this.intervalId = setInterval(() => {
                if (this.remainingTime > 0) {
                    this.remainingTime--;
                } else {
                    clearInterval(this.intervalId);
                }

                if (this.remainingTime == 300) {
                    this.$notify({
                        title: 'تنبــــــيه',
                        message: 'متبقي أقل من خمسة دقائق على إنتهاء الوقت',
                        position: 'bottom-right',
                        showClose: false,
                        type: 'warning'
                    });
                }

                if (this.remainingTime == 120) {
                    this.$notify({
                        title: 'تنبــــــيه',
                        message: 'متبقي أقل من خمسة دقائق على إنتهاء الوقت سيتم ارسال الأجوبة تلقائيا',
                        position: 'bottom-right',
                        showClose: false,
                        type: 'warning'
                    });
                }

                if (this.remainingTime == 1) {
                    this.ForceSubmitExam();
                }

            }, 1000);
        },
        pad(value) {
            return value.toString().padStart(2, '0');
        },


        OpenLecturePDF(path) {
            this.SelectedPDFPath = this.ServerUrl+path;
            this.LectureOrExam = 40;
        },



        StartExamA() {
            if (this.SelectedExam.hasLimght) {
                this.remainingTime = this.SelectedExam.limght * 60;
                this.StartExam = true;
            } else {
                this.StartExam = true;
            }
            
            
        },
        SubmitExam() {
            Swal.fire({
                title: 'هل أنت متأكد من الإنتهاء من الإختبار  ؟',
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: `تأكيد العملية`,
                denyButtonText: `الغاء العملية`,
            }).then((result) => {
                if (result.isConfirmed) {
                    this.Start();
                    this.$http.SubmitExam(this.SelectedExam.questions)
                        .then(response => {
                            this.Stop();
                            this.$helper.ShowMessage('success', 'تمت العملية بنجاح ', response.data);
                            this.LectureOrExam = 0;
                        })
                        .catch((err) => {
                            this.Stop();
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


        ForceSubmitExam() {
            this.Start();
            this.$http.SubmitExam(this.SelectedExam.questions)
                .then(response => {
                    this.Stop();
                    this.$helper.ShowMessage('success', 'إنتهي الوقت المحدد للإمتحان ، تم إرسال الأجوبة ', response.data);
                    this.LectureOrExam = 0;
                })
                .catch((err) => {
                    this.Stop();
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
        },

       


    },
    beforeDestroy() {
        clearInterval(this.intervalId);
    },
}
