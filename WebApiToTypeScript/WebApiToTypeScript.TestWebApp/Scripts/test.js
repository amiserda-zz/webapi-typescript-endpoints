
var promise = new Promise((resolve, reject) => {
    resolve($.get('api/testapi/get'));
})

promise.then(function(data) {
    console.log(data);
})