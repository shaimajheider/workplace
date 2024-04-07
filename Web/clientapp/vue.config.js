module.exports = {
    chainWebpack: config => {
        config.module
            .rule('js')
            .use('thread-loader')
            .loader('thread-loader')
            .tap(options => {
                if (options === undefined) {
                    options = {};
                }
                options.parallel = false;
                return options;
            });
    }
};