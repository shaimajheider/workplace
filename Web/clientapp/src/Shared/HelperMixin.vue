<script>
    import blockUI from './BlockUIService.js';
    import DataService from './DataService.js';
    export default {
        data() {
            return {
                loginDetails: null,

                Cities: [],
                Facilities: [],

                
            }
        },
        methods: {
            async GetCities() {
                this.Cities = [],
                blockUI.Start();
                try {
                    const res = await DataService.GetCities();
                    this.Cities = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },
            async GetFacilities(id) {
                this.Facilities = [],
                blockUI.Start();
                try {
                    const res = await DataService.GetFacilities(id);
                    this.Facilities = res.data.info;
                    blockUI.Stop();
                } catch (err) {
                    blockUI.Stop();
                }
            },
        }
    }
</script>
