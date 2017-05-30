var Api;
(function (Api) {
    class NewTestApiService {
        constructor() {
            this.get = (a, b, c, d, e, f, g, h, i, j, k, l, m) => {
                return new Promise((resolve) => resolve($.ajax({ method: 'GET', url: '/api/testapi/get', data: { a, b, c, d, e, f, g, h, i, j, k, l, m } })));
            };
            this.post = (a, b, c, d, e, f, g, h, i, j, k, l, m) => {
                return new Promise((resolve) => resolve($.ajax({ method: 'POST', url: '/api/testapi/post', data: { a, b, c, d, e, f, g, h, i, j, k, l, m } })));
            };
        }
    }
    Api.NewTestApiService = NewTestApiService;
    class TestApiService {
        constructor() {
            this.get2 = (i, s) => {
                return new Promise((resolve) => resolve($.ajax({ method: 'GET', url: '/api/testapi/get2', data: { i, s } })));
            };
            this.get1 = () => {
                return new Promise((resolve) => resolve($.ajax({ method: 'GET', url: '/api/testapi/get1' })));
            };
        }
    }
    Api.TestApiService = TestApiService;
})(Api || (Api = {}));
//# sourceMappingURL=Api.js.map