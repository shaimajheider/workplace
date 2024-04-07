import HelperMixin from '../../../Shared/HelperMixin.vue'

export default {
    name: "appHeader",
    mixins: [HelperMixin],
    async created() {
        window.scrollTo(0, 0);
        await this.CheckLoginStatus();
    },
    data() {
        return {
            //loginDetails: null,
            active: 1,
            menuFlag: [20],
            Tog: 'navbar-toggle',
        };
    },

    methods: {

        IsTogled() {
            if (this.Tog == 'navbar-toggle toggled') {
                this.Tog = 'navbar-toggle';
            } else {
                this.Tog = 'navbar-toggle toggled';
            }
        },

        OpenMenuByToggle() {
            var root = document.getElementsByTagName("html")[0]; // '0' to assign the first (and only `HTML` tag)
            var classes = root.getAttribute("class");
            if (classes == "g-sidenav-show bg-gray-100 g-sidenav-hidden") {
                root.setAttribute("class", "g-sidenav-show bg-gray-100 g-sidenav-pinned");
            }
            else if (classes == null) {
                root.setAttribute("class", "g-sidenav-show bg-gray-100 g-sidenav-pinned");
            }
            else {
                root.setAttribute("class", "g-sidenav-show bg-gray-100 g-sidenav-hidden");
            }
        },


        Logout() {
            window.location.href = "/Login";
        },

        href(url) {
            this.$router.push(url);
        },

        //CheckLoginStatus() {
        //  try {
        //    this.loginDetails = JSON.parse(
        //      localStorage.getItem("currentUser-client")
        //    );
        //    if (this.loginDetails == null) {
        //      window.location.href = "/Login";
        //    }
        //  } catch (error) {
        //    window.location.href = "/Login";
        //  }
        //},

        openNav() {
            document.getElementById("mySidebar").style.width = "250px";
            document.getElementById("main").style.marginLeft = "250px";
        },

        /* Set the width of the sidebar to 0 and the left margin of the page content to 0 */
        closeNav() {
            document.getElementById("mySidebar").style.width = "0";
            document.getElementById("main").style.marginLeft = "0";
        },
    },
};
