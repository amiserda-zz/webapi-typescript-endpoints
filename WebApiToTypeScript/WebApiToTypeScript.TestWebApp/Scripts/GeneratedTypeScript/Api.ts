module Api {
    export class TestApiService {

        get1 = () : Promise<string> => {
            return new Promise<string>((resolve) => resolve($.get('/api/testapi/Get1', {})));
        }
        get12 = (i: number, s: string) : Promise<number> => {
            return new Promise<number>((resolve) => resolve($.get('/api/testapi/Get12', {i, s})));
        }
    }
}
