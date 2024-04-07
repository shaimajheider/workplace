<script>
    import blockUI from './BlockUIService.js';
    import DataService from './DataService.js';
    import CryptoJS from 'crypto-js';
    export default {
        data() {
            return {
                loginDetails: null,

                PaymentMethods: [],
                CompaniesRooms: [],
                

                
                /*Local*/
                ServerUrl: 'http://localhost:5000',
                PlatFormPass: 'PassKeyPass',


                WebSiteName: 'مســـــاحتي',
                SystemName: 'مساحتي',
                logoPath:'/assets/img/1/logo.png',




            }
        },
        methods: {

            encrypt: function encrypt(data, SECRET_KEY) {
                var dataSet = CryptoJS.AES.encrypt(JSON.stringify(data), SECRET_KEY);
                dataSet = dataSet.toString();
                return dataSet;
            },
            decrypt: function decrypt(data, SECRET_KEY) {
                data = CryptoJS.AES.decrypt(data, SECRET_KEY);
                data = JSON.parse(data.toString(CryptoJS.enc.Utf8));
                return data;
            },


            //////////////////////////////////////////////// Auth //////////////////////////////

            //async CheckLoginStatus() {
            //    try {
            //        this.loginDetails = JSON.parse(this.decrypt(localStorage.getItem('currentUser-client'), this.PlatFormPass));
            //        if (this.loginDetails != null) {
            //            //window.location.href = '/Login';
            //        }
            //    } catch (error) {
            //        //window.location.href = '/Login';
            //    }
            //},

            async CheckLoginStatus() {
                try {
                    this.loginDetails = JSON.parse(this.decrypt(localStorage.getItem('currentUser-client'), this.PlatFormPass));
                    if (this.loginDetails != null) {
                        const res = await DataService.IsLoggedin();
                        if (!res.data)
                            this.logout();
                    } else {
                        this.logout();
                    }
                } catch (error) {
                    this.logout();
                }
            },

            async logout() {
                localStorage.removeItem('currentUser-client');
                localStorage.clear();
                document.cookie.split(";").forEach(function (c) { document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/"); });
                this.$http.Logout()
                    .then(() => {
                        window.location.href = "/Login";
                    })
            },


            async GetPaymentMethods() {
                this.PaymentMethods = [],
                blockUI.Start();
                try {
                    const res = await DataService.GetAllPaymentMethods();
                    this.PaymentMethods = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },

            async GetCompaniesRooms() {
                this.CompaniesRooms = [],
                blockUI.Start();
                try {
                    const res = await DataService.GetCompaniesRooms();
                    this.CompaniesRooms = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },


            //async GetFacilities(CityId) {
            //    this.Facilities  = [],
            //    blockUI.Start();
            //    try {
            //        const res = await DataService.GetAllFacilities(CityId);
            //        this.Facilities = res.data.info;
            //        blockUI.Stop();
            //    } catch (err) {
            //        blockUI.Stop();
            //    }
            //},
            //async GetPorts() {
            //    this.Ports = [],
            //    blockUI.Start();
            //    try {
            //        const res = await DataService.GetAllPorts();
            //        this.Ports = res.data.info;
            //        blockUI.Stop();
            //    } catch (err) {
            //        blockUI.Stop();
            //    }
            //},

            //async GetAgencies() {
            //    this.Agencies = [],
            //    blockUI.Start();
            //    try {
            //        const res = await DataService.GetAllAgencies();
            //        this.Agencies = res.data.info;
            //        blockUI.Stop();
            //    } catch (err) {
            //        blockUI.Stop();
            //    }
            //},

            //async GetDocumentsReleaseResones() {
            //    this.DocumentsReleaseResones = [],
            //    blockUI.Start();
            //    try {
            //        const res = await DataService.GetAllDocumentsReleaseResones();
            //        this.DocumentsReleaseResones = res.data.info;
            //        blockUI.Stop();
            //    } catch (err) {
            //        blockUI.Stop();
            //    }
            //},

            //async GetIssuesType() {
            //    this.IssuesType = [],
            //    blockUI.Start();
            //    try {
            //        const res = await DataService.GetAllIssuesType();
            //        this.IssuesType = res.data.info;
            //        blockUI.Stop();
            //    } catch (err) {
            //        blockUI.Stop();
            //    }
            //},

            //async GetProceduresTakens() {
            //    this.ProceduresTakens = [],
            //    blockUI.Start();
            //    try {
            //        const res = await DataService.GetAllProceduresTakens();
            //        this.ProceduresTakens = res.data.info;
            //        blockUI.Stop();
            //    } catch (err) {
            //        blockUI.Stop();
            //    }
            //},


            //async GetArrestedCases() {
            //    this.ArrestedCases = [],
            //    blockUI.Start();
            //    try {
            //        const res = await DataService.GetAllArrestedCases();
            //        this.ArrestedCases = res.data.info;
            //        blockUI.Stop();
            //    } catch (err) {
            //        blockUI.Stop();
            //    }
            //},

            



        }
    }
</script>
