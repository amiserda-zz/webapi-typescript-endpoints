module Api {
    export class NewTestApiService {

        get = (a: boolean, b: number, c: string, d: number, e: number, f: any, g: number, h: number, i: number, j: number, k: number, l: number, m: number) : Promise<Date> => {
            return new Promise<Date>((resolve) => resolve($.ajax({method: 'GET',url: '/api/testapi/get',data: {a, b, c, d, e, f, g, h, i, j, k, l, m}})));
        }
        post = (a: boolean, b: number, c: string, d: number, e: number, f: any, g: number, h: number, i: number, j: number, k: number, l: number, m: number) : Promise<any> => {
            return new Promise<any>((resolve) => resolve($.ajax({method: 'POST',url: '/api/testapi/post',data: {a, b, c, d, e, f, g, h, i, j, k, l, m}})));
        }
    }
    export class TestApiService {

        get2 = (i: number, s: string) : Promise<number> => {
            return new Promise<number>((resolve) => resolve($.ajax({method: 'GET',url: '/api/testapi/get2',data: {i, s}})));
        }
        get1 = () : Promise<string> => {
            return new Promise<string>((resolve) => resolve($.ajax({method: 'GET',url: '/api/testapi/get1'})));
        }
    }
}
