import AppHeader from './AppHeader/AppHeader.vue';
import AppFooter from './AppFooter/AppFooter.vue';
import Dashboard from './Dashboard/Dashboard.vue';

export default {
    name: 'layout',   
    components: {
        'app-header': AppHeader,
        'app-footer': AppFooter,
        'app-dasboard': Dashboard,
       
    },
    created() {
        this.IsLoggedin();
        this.$blockUI.$loading = this.$loading;
    },
    data() {
        return {
        
        };
    },
    methods: {
        IsLoggedin() {
            var $this = this;
            $this.$http.IsLoggedin()
                .then(response => {
                    $this.isAuthenticated = response.data;
                    if (!$this.isAuthenticated) {
                        window.location.href = '/Login';
                    } else {
                        this.isActive = true;
                    }
                })
                .catch((err) => {
                    return err;
                });
        },
       
    }
}
