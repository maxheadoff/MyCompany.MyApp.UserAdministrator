/*
const loginRoutine = user => new Promise((resolve, reject) => {
  axios({ url: 'auth', data: user, method: 'POST' })
    .then(resp => {
      const token = resp.data.token
      localStorage.setItem('user-token', token) // store the token in localstorage
      resolve(resp)
    })
    .catch(err => {
      localStorage.removeItem('user-token') // if the request fails, remove any possible user token if possible
      reject(err)
    })
})
*/

const loginRoutine = function (user, token) {
  $cookies.set('user-token', token); // store the token in localstorage
  $cookies.set('user-login', user);
  $cookies.set('isAuthenticated', true);
  console.log('user:' + user + ' logged');
}

const logoutroutine = () => {
  $cookies.remove('user-token');
  $cookies.remove('user-login');
  $cookies.set('isAuthenticated', false);
}

module.exports = loginRoutine;

