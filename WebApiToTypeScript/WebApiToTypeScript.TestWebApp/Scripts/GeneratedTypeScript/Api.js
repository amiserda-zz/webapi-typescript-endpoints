var Api;
(function (Api) {
    class TestApiService {
        constructor() {
            this.get1 = () => {
                return new Promise((resolve) => resolve($.get('/api/testapi/Get1', {})));
            };
            this.get12 = (i, s) => {
                return new Promise((resolve) => resolve($.get('/api/testapi/Get12', { i, s })));
            };
        }
    }
    Api.TestApiService = TestApiService;
})(Api || (Api = {}));
//# sourceMappingURL=Api.js.map