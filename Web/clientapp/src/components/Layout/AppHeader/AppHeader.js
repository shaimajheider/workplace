
import HelperMixin from '../../../Shared/HelperMixin.vue'
export default {
    name: 'AppHeader',
    mixins: [HelperMixin],
    async created() {
    },
    data() {
        return {  
            //loginDetails: null,
            Info: [],
            TempInfo: [],
            Count: 0,
        };
    },
  
    methods: {

        href(url) {
            this.$router.push(url);
        },

        ChangeMobileSecition() {

            if (document.getElementById("MobileMenuSection").classList.contains('active')) {
                document.getElementById("MobileMenuSection").classList.remove('active');
            } else {
                document.getElementById("MobileMenuSection").classList.add('active');
            }
        },

        ChangeMobileSecionsUserProfile() {
            if (document.getElementById("UserProfile").classList.contains('open')) {
                document.getElementById("UserProfile").classList.remove('open');
                document.getElementById("UserProfileMenu").style.display = "none";
            } else {
                document.getElementById("UserProfile").classList.add('open');
                document.getElementById("UserProfileMenu").style.display = "block";
            }
        },


        //encrypt: function encrypt(data, SECRET_KEY) {
        //    var dataSet = CryptoJS.AES.encrypt(JSON.stringify(data), SECRET_KEY);
        //    dataSet = dataSet.toString();
        //    return dataSet;
        //},
        //decrypt: function decrypt(data, SECRET_KEY) {
        //    data = CryptoJS.AES.decrypt(data, SECRET_KEY);
        //    data = JSON.parse(data.toString(CryptoJS.enc.Utf8));
        //    return data;
        //},

        //CheckLoginStatus() {
        //    try {
        //        this.loginDetails = JSON.parse(this.decrypt(localStorage.getItem('currentUser-client'), this.PlatFormPass));
        //        if (this.loginDetails != null) {
        //            //window.location.href = '/Login';
        //        }
        //    } catch (error) {
        //        //window.location.href = '/Login';
        //    }
        //},

        Logout() {
            window.location.href = "/Login";
        },

        playSound() {
            const audio = new Audio("windows8_email_notif.mp3");
            audio.play();
        }
    }    
}
