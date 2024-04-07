export default {
    name: 'appHeader',    
    created() { 
        window.scrollTo(0, 0);
        this.CheckLoginStatus();
    },
    data() {
        return {      
            loginDetails: null,
            active: 1,
            menuFlag: [20],
            
        };
    }, 
  
    methods: {

        Logout() {
            window.location.href = "/Login";
        },


        href(url) {
            this.$router.push(url);
        },

        CheckLoginStatus() {
            try {
                this.loginDetails = JSON.parse(localStorage.getItem('currentUser-client'));
                if (this.loginDetails == null) {
                    window.location.href = '/Login';
                }
            } catch (error) {
                window.location.href = '/Login';
            }
        },

        OpenMenuByToggle() {
            var root = document.getElementsByTagName("body")[0]; // '0' to assign the first (and only `HTML` tag)
            if (root.getAttribute("class") == "rtl sidebar-mini rtl-active") {
                root.setAttribute("class", "rtl rtl-active");
            } else {
                root.setAttribute("class", "rtl sidebar-mini rtl-active");
            }
        },


    }    
}
