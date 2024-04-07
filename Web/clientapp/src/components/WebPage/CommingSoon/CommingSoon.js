export default {
    name: 'home',
    components: {
      
    },
    created() {   
        window.scrollTo(0, 0);
    },
    data() {
        return {
        };
    },
    methods: {

        href(url) {
            this.$router.push(url);
        },

    }    
}
