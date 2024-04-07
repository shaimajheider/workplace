<template>

        <router-view></router-view>
    
</template>
<script>
    export default {
        name: 'App',
        components: {
        },
        created() {
         //   this.$blockUI.$loading = this.$loading;
         //   this.IsLoggedin();
        },
        data() {
            return {
                isAuthenticated: false,
                isActive: false,
            };
        },
        methods: {
            //IsLoggedin() {
            //    this.$http.IsLoggedin()
            //    .then(response => {
            //        this.isAuthenticated = response.data;
            //        var route = window.location.href.split("/")[3]; 
            //        if (!this.isAuthenticated) {
            //            if (route != 'Login') {
            //                window.location.href = "/Login";
            //            }
            //        } 
            //    })
            //        .catch((err) => {
            //            this.$blockUI.Stop();
            //        return err;
            //    });
            //},
        }
    }
</script>