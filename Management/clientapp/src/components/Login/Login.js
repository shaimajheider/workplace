import CryptoJS from 'crypto-js';
import HelperMixin from '../../Shared/HelperMixin.vue'

export default {
    name: 'Login',
    mixins: [HelperMixin],
    components: {
    },
    created() {
        this.$blockUI.$loading = this.$loading;
        this.logout();
    },
    data() {
        return {
            isAuthenticated: false,
            isActive: false,
            form: {
                Password: null,
                Email: null
            }
        };
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


        logout() {
            localStorage.removeItem('currentUser-client');
            document.cookie.split(";").forEach(function (c) { document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/"); });
            this.$blockUI.Start();
            this.$http.Logout()
                .then(() => {
                    this.$blockUI.Stop();
                    //  window.location.href = "/Login";
                })
                .catch((err) => {
                    this.$blockUI.Stop(err);
                    //console.error(err);
                });
        },

        login() {
            if (!this.form.Email) {
                this.$notify({
                    title: 'خطأ',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + 'الرجاء إدخال البريد الإلكتروني' + '</strong>',
                    type: 'error'
                });
                return;
            }
            if (!this.form.Password) {
                this.$notify({
                    title: 'خطأ',
                    dangerouslyUseHTMLString: true,
                    message: '<strong>' + 'الرجاء إدخال الرقم السري' + '</strong>',
                    type: 'error'
                });
                return;
            }

            //this.Loading = true;
            this.$blockUI.Start();
            this.$http.login(this.form)
                .then(response => {
                    //debugger;
                    this.$blockUI.Stop();
                    localStorage.setItem('currentUser-client', this.encrypt(JSON.stringify(response.data), this.PlatFormPass));
                    window.location.href = '/';
                })
                .catch((error) => {
                    this.$blockUI.Stop();
                    //$blockUI.close();
                    //debugger;
                    // this.Loading = false;
                    this.$notify({
                        title: 'خطأ',
                        dangerouslyUseHTMLString: true,
                        message: '<strong>' + error.response.data + '</strong>',
                        type: 'error'
                    });
                });
        }
    }
}
