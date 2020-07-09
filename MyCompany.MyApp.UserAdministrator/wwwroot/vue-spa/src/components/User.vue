<template>
  <div>
    <div class="item__show">
      <div class="error-message">
        <p v-show="!success">Error sending data to server,please, try later. Message:{{error_message}} <input type="button" value="x" v-on:click="clean" /> </p>
      </div>
      <div class="item__title" v-model="user"><p>User id:{{user.id}}, login:{{user.login}}</p></div>
      <input class="del_button" type="button" v-on:click="remove_user_toggle=!remove_user_toggle"  value="" :disabled="isNew"/>
      <input v-show="!edit_mode" class="item_button" type="button" value="" v-on:click="edit" />
      <input v-show="!roles_edit_mode" class="role_button" type="button" value="" v-on:click="roles_edit"  :disabled="isNew"/>
    </div>
    <div>
      <form v-show="remove_user_toggle" class="item__form" @submit.prevent="remove_user">
        <fieldset>
          <legend>Are you sure?</legend>
          <p id="attention">Are you really wont to delete user:{{user.name}}? </p>
          <div class="buttons_group">
            <input type="submit" class="submit_button" value="Save">
            <input type="button" value="Cancel" class="cancel_button" v-on:click="remove_user_toggle=!remove_user_toggle">
          </div>
          </fieldset>
      </form>
      <form v-show="roles_edit_mode" class="item__form" @submit.prevent="submit_roles">
        <fieldset>
          <div>
            <ul>
              <li v-for="role in roles">
                <input type="checkbox" :id="role.id" :value="role" v-model="user.roles">
                <label :for="role.name">{{role.name}}</label>
              </li>
            </ul> 
          </div>
          <div class="buttons_group">
            <input type="submit" class="submit_button" value="Save">
            <input type="button" value="Cancel" class="cancel_button" v-on:click="roles_edit">
          </div>
        </fieldset>
      </form>
      <form v-show="edit_mode" class="item__form" @submit.prevent="submit">
        
        <fieldset>
          <div class="error-message">
            <p v-show="!valid">{{error_message}}</p>
            <p v-show="!success">Error sanding data to server,please, try later. Message:{{error_message}} </p>
          </div>
          <!--<legend>User id:{{user.id}}, login:{{user.login}}</legend>-->
          <div class="input_field">
            <label class="label" for="name">Name</label>
            <input type="text" name="name" id="name" required="" v-model="user.name">
          </div>
          <div v-show="isNew" class="input_field">
            <label class="label" for="login">Login</label>Rjc
            <input type="text" name="login" id="login" required="" v-model="user.login">
          </div>
          <div class="input_field">
            <label class="label" for="password">Password</label>
            <input type="password" name="password" id="password" required="" v-model="user.password">
          </div>
          <div class="input_field">
            <label class="label" for="conf_password">Confirm password</label>
            <input type="password" name="conf_password" id="conf_password" required="" v-model="conf_password">
          </div>
          <div class="input_field">
            <label class="label" for="email">Email</label>
            <input type="text" name="email" id="email" required="" v-model="user.email">
          </div>
          <div class="buttons_group">
            <input type="submit" class="submit_button" value="Save">
            <input type="button" class="cancel_button" value="Cancel" v-on:click="edit">
          </div>
        </fieldset>
      </form>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';
  
  var emailRegExp = /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/;
  export default {
    name: 'user',
    props: ['user_id'],
    data() {
      return {
        JWTToken: null,
        user: { name: '', login: '', id: this.user_id },
        valid: true,
        success: true,
        error_message:null,
        endpoint: 'https://localhost:44378/api/users/',
        roles_endpoint: 'https://localhost:44378/api/roles/',
        edit_mode: false,
        roles_edit_mode: false,
        roles: null,
        selectedRoles: [],
        conf_password: '',
        remove_user_toggle: false // show|hide delete form
      }
    },
    computed: {
      isNew: function () {
        return this.user.id===0;
      },
     },
    methods: {
      getUser(id) {
        axios(this.endpoint + id, { headers: { "Authorization": `Bearer ${this.JWTToken}` } })
          .then(response => {
            this.user = response.data;
          })
          .catch(error => {
            console.log(error)
          })
      },
      getRoles: function() {
        axios(this.roles_endpoint, { headers: { "Authorization": `Bearer ${this.JWTToken}` } })
          .then(response => {
            this.roles = response.data
          })
          .catch(error => {
            console.log(error)
          })
      },
      validate: function (type,value) {
        if (type === "email") {
          this.valid = this.isEmail(value);
        }
        else
          if (type === "password") {
            var res = this.user.password === this.conf_password;
            if (!res)
              this.error_message = "Please,check password";
            this.valid = (res) ? true : false;
        }
      },
      // check for valid email adress
      isEmail: function (value) {
        var res = emailRegExp.test(value);
        if (!res)
          this.error_message = "Please, enter valid Email";
        return res;
      },
      edit: function () {
        this.edit_mode = !this.edit_mode;
      },
      roles_edit: function () {
        this.roles_edit_mode = !this.roles_edit_mode;
      },
      submit: function () {
        if (!this.valid)
          return;
        //this.user.roles = this.selectedRoles;
        if (this.user_id > 0) {
          axios.put(this.endpoint + this.user_id, this.user,
            { headers: { "Authorization": `Bearer ${this.JWTToken}` } })
            .then(() => {
              console.log('user saved');
              this.success = true;
              this.$emit('refresh', this.user.id);
              this.edit();
            })
            .catch(err => {
              console.log(err);
              this.error_message = err;
              this.success = false;
              this.$emit('refresh', this.user.id);
            })
        }
        else {
          axios.post(this.endpoint, this.user,
            { headers: { "Authorization": `Bearer ${this.JWTToken}` } })
            .then(response => {
              console.log('user created');
              this.success = true;
              this.user = response.data;
            //  this.user_id = this.user.id;
              console.log('user:' + this.user.name);
              console.log('user.id:' + this.user.id);
              this.edit();
              this.$emit('refresh', this.user.id);
            })
            .catch(err => {
              console.log(err);
              this.error_message = err;
              this.success = false;
              this.$emit('refresh', this.user.id);
            })
        }
        
      },
      submit_roles: function () {
        var roles = [];
        this.user.roles.forEach(item => {
          roles.push(item.id);
        });
        axios.put(this.endpoint + this.user_id + '/roles/', roles,
          { headers: { "Authorization": `Bearer ${this.JWTToken}` } })
          .then(() => {
            console.log('user roles saved');
            this.success = true;
            this.roles_edit_mode = false;
          })
          .catch(err => {
            console.log(err);
            this.error_message = err;
            this.success = false;
          })
      },
      remove_user: function () {
        axios.delete(this.endpoint + this.user_id,
          { headers: { "Authorization": `Bearer ${this.JWTToken}` } })
          .then(()=> {
            console.log('user deleted');
            this.success = true;
            this.remove_user_toggle = false;
            this.$emit('user_deleted', this.user_id);
          })
          .catch(err => {
            console.log(err);
            this.error_message = err;
            this.success = false;
          })
      },
      clean: function () {
        this.success = true;
      }
    },
    created() {
      this.JWTToken = $cookies.get('user-token');
      if (this.user_id > 0)
        this.getUser(this.user_id);
      else
        this.edit_mode = true;
    },
    watch: {
      '$route'() {
        this.getUser(this.id);
      },
      "user.email": function (value) {
        this.validate("email", value);
      },
      'roles_edit_mode': function (value) {
        if (value)
          this.getRoles();
      },
      'conf_password': function (value) {
        this.validate("password", value);
      }
     }
  }
</script>
<style>
 
   .error-message p {
    background: #e94b35;
    color: #ffffff;
    font-size: 1.1rem;
    text-align: center;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    border-radius: 0.25em;
    padding: 3px;
    margin:4px;
 
  }

  .del_button {
    background-image: url(../assets/criss-cross.png);
  }
  .item_button {
    background-image: url(../assets/edit.png);
  }
  .role_button {
    background-image: url(../assets/role.png);
  }

  .buttons_group input {
    background-position: left;
    background-repeat: no-repeat;
    background-color: transparent;
    background-size: 30px;
    text-align:right;
    cursor: pointer; 
    vertical-align: middle;
    border: none;
    width: 100px;
    height: 40px;
    margin: 3px;
    margin-right:20px;
    padding:10px;
  }
    .buttons_group input:hover {
        border: 2px outset green;
        border-radius:10px;
        font-weight:bold;
        background-color:beige;
        
    }
    .submit_button {
    background-image: url(../assets/accept.png);
  }
  .cancel_button {
    background-image: url(../assets/close.png);
  }

  .item__show {
    min-width: 500px;
    text-align: center;
    align-content: center;
    background-color: antiquewhite;
    margin-bottom: 2px;
  }
    .item__show input {
      background-position: center;
      background-repeat: no-repeat;
      background-color: transparent;
      background-size: contain;
      cursor: pointer; 
      vertical-align: middle;
      border: none;
      width:25px;
      height:20px;
      margin:3px;
    }
  .item__title {
    float: left;
    font-size: 17px;
    font-style: italic;
    font-weight: bold;
    min-width: 400px;
  }

  .item__title p {
      margin:0px;
  }

  .item__form {
    min-width: 500px;
    text-align: left;
    align-content: center;
    background-color: antiquewhite;
    margin-bottom: 2px;
  }
  #attention{
      color:red;
  }
  li {
    list-style-type: none;
  }
  ul {
    margin-left: 0; 
    padding-left: 0;
  }
  .input_field {
    width: 50%;
    margin: 10px;
  }


</style>
