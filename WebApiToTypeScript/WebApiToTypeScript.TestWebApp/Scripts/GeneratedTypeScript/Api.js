var Api;
(function (Api) {
    class NewTestApiService {
        constructor() {
            this.get = (a, b, c, d, e, f, g, h, i, j, k, l, m) => {
                return new Promise((resolve) => resolve($.get('/api/testapi/get', { a, b, c, d, e, f, g, h, i, j, k, l, m })));
            };
        }
    }
    Api.NewTestApiService = NewTestApiService;
    class TestApiService {
        constructor() {
            this.get2 = (i, s) => {
                return new Promise((resolve) => resolve($.get('/api/testapi/get2', { i, s })));
            };
            this.get1 = () => {
                return new Promise((resolve) => resolve($.get('/api/testapi/get1', {})));
            };
        }
    }
    Api.TestApiService = TestApiService;
})(Api || (Api = {}));
//# sourceMappingURL=Api.js.map