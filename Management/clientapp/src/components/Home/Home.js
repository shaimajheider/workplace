/*import swal from "sweetalert";*/


import ReportsDoughnutChart from "../Charts/ReportsDoughnutChart.vue";
import GradientLineChart from "../Charts/GradientLineChart.vue";
import ThinBarChart from "../Charts/ThinBarChart.vue";
import HelperMixin from '../../Shared/HelperMixin.vue'


export default {
    name: 'home',
    mixins: [HelperMixin],
    components: {
        ReportsDoughnutChart,
        GradientLineChart,
        ThinBarChart,
    },
    //prop: {
    //    chartTest: ['chartTest.Chart'],
    //},
    async created() {
        window.scrollTo(0, 0);

        this.GetInfo();

        //setInterval(() => this.GetInfo(), 1800);
        //this.GetInfo2();
        await this.CheckLoginStatus();
    },
    data() {
        return {

            ChartBerMonth:null,
            ChartBerCompany:null,
            ChartBerDay: null,
            todayUsed: 0,
            pieChartCount: { number: 1, text: 'جهاز' },
            

            //chartIfno:
            //{
            //    labels: ['Jan', 'Fep', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            //    datasets: [
            //        {
            //                label: 'عدد مرات الغسيل',
            //                data: [50, 40, 300, 220, 500, 250, 400, 230, 500, 400, 230, 500],
            //        }
            //    ]
            //},


            //Chart: {
            //    labels: ['Nipro', ' B.Barun', 'Fresenius', 'Dia Life', 'Baxter'],
            //    datasets: {
            //        label: ['Nipro', ' B.Barun', 'Fresenius', 'Dia Life', 'Baxter'],
            //        data: [89, 20, 13, 32, 20],
            //    },
            //},

            //berDay: {
            //    labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
            //    datasets: {
            //        label: 'عدد مرات الغسيل',
            //        data: [150, 230, 380, 220, 420, 200, 70, 500],
            //    },
            //},






            pageNo: 1,
            pageSize: 5,
            pages: 0,
            state: 0,

            //loginDetails: null,
            Info: [],
            Info2: [],
        ElSteps:0,
            ruleForm: {
                RegistryNumber: "",
                NationalId: "",
                Phone: "",
                Recaptcha: "",
            },
            Percentage: 0,
            loading: false,
            Voters: {},
            success: {
                confirmButtonText: "OK",
                type: "success",
                dangerouslyUseHTMLString: true,
                center: true,
            },
            error: {
                confirmButtonText: "OK",
                type: "error",
                dangerouslyUseHTMLString: true,
                center: true,
            },
            warning: {
                confirmButtonText: "OK",
                type: "warning",
                dangerouslyUseHTMLString: true,
                center: true,
            },    
        };
    },
    methods: {

        //chartIfno: {
        //    labels: [
        //        'Jan',
        //        'Fep',
        //        'Mar',
        //        'Apr',
        //        'May',
        //        'Jun',
        //        'Jul',
        //        'Aug',
        //        'Sep',
        //        'Oct',
        //        'Nov',
        //        'Dec'
        //    ],
        //    datasets: [
        //        {
        //            label: 'عدد مرات الغسيل',
        //            data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
        //        }
        //    ]
        //},

        GetInfo() {
            this.$blockUI.Start();
            this.$http.GetDashboardInfo()
                .then(response => {
                    this.$blockUI.Stop();
                    //debugger

                    this.Info = response.data.info;

                    //this.ChartBerCompany = this.Info.perCompany;
                    //this.ChartBerDay = this.Info.perDay;
                    //this.ChartBerMonth = this.Info.chartObjList;
                    ////console.log(this.ChartBerCompany)
                    //this.todayUsed = this.Info.todayUsed;
                    //const i = this.ChartBerCompany.datasets.data.reduce((a, b) => a + b, 0);
                    //this.pieChartCount = { number: i, text: 'جهاز' }
                    //this.ChartBerMonth={
                    //    labels: response.data.info.chartObjList.labels,
                    //    datasets: {
                    //        label: response.data.info.chartObjList.datasets[0].label,
                    //        data: response.data.info.chartObjList.datasets[0].data
                    //    }
                    //}

                //    Chart: {
                //        labels: ['Nipro', ' B.Barun', 'Fresenius', 'Dia Life', 'Baxter'],
                //            datasets: {
                //            label: ['Nipro', ' B.Barun', 'Fresenius', 'Dia Life', 'Baxter'],
                //                data: [89, 20, 13, 32, 20],
                //},
                //    },

                //    berDay: {
                //        labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
                //            datasets: {
                //            label: 'عدد مرات الغسيل',
                //                data: [150, 230, 380, 220, 420, 200, 70, 500],
                //},
                //    },

                    

                    //this.ChartBerCompany = {
                    //    labels: response.data.info.perCompany.labels,
                    //    datasets: {
                    //        label: response.data.info.perCompany.datasets[0].label,
                    //        data: response.data.info.perCompany.datasets[0].data
                    //    }
                    //}


                    //this.ChartBerDay = {
                    //    labels: response.data.info.perDay.labels,
                    //    datasets: {
                    //        label: response.data.info.perDay.datasets[0].label,
                    //        data: response.data.info.perDay.datasets[0].data
                    //    }
                    //}

                    //ChartBerMonth: null,
                    //    ChartBerCompany: null,
                    //        ChartBerDay: null,




                })
                .catch(() => {
                    this.$blockUI.Stop();
                });
        },



    }    
}
