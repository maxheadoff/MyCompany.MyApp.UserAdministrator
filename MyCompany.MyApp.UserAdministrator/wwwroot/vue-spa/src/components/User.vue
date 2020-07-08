<template>
  <div>
    <div class="item__show">
      <div class="error-message">
        <p v-show="!success">Error sanding data to server,please, try later. Message:{{error_message}} <input type="button" value="x" v-on:click="clean" /> </p>
      </div>
      <div class="item__title" v-model="user">User id:{{user.id}}, login:{{user.login}}</div>
      <input v-show="!edit_mode" class="item_button" type="button" value="Delete" v-on:click="remove_user"  :disabled="isNew"/>
      <input v-show="!edit_mode" class="item_button" type="button" value="Edit" v-on:click="edit" />
      <input v-show="!roles_edit_mode" class="item_button" type="button" value="Roles" v-on:click="roles_edit"  :disabled="isNew"/>
    </div>
    <div>
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
          <div>
            <input type="submit" value="Save">
            <input type="button" value="Cancel" v-on:click="roles_edit">
          </div>
        </fieldset>
      </form>
      <form v-show="edit_mode" class="item__form" @submit.prevent="submit">
        <div class="error-message">
          <p v-show="!valid">Please enter a valid email address.</p>
          <p v-show="!success">Error sanding data to server,please, try later. Message:{{error_message}} </p>
        </div>
        <fieldset>
          <!--<legend>User id:{{user.id}}, login:{{user.login}}</legend>-->
          <div>
            <label class="label" for="name">Name</label>
            <input type="text" name="name" id="name" required="" v-model="user.name">
          </div>
          <div v-show="isNew">
            <label class="label" for="login">Login</label>
            <input type="text" name="login" id="login" required="" v-model="user.login">
          </div>
          <div>
            <label class="label" for="email">Email</label>
            <input type="text" name="email" id="email" required="" v-model="user.email">
          </div>
          <div>
            <input type="submit" value="Save">
            <input type="button" value="Cancel" v-on:click="edit">
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
        user: { name: '', login: '', id: 0 },
        valid: true,
        success: true,
        error_message:null,
        endpoint: 'https://localhost:44378/api/users/',
        roles_endpoint: 'https://localhost:44378/api/roles/',
        edit_mode: false,
        roles_edit_mode: false,
        roles: null,
        selectedRoles:[]
      }
    },
    computed: {
      isNew: function () {
        return this.user_id===0;
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
          this.valid = this.isEmail(value) ? true : false;
        }
      },
      // check for valid email adress
      isEmail: function (value) {
        return emailRegExp.test(value);
      },
      edit: function () {
        this.edit_mode = !this.edit_mode;
      },
      roles_edit: function () {
        this.roles_edit_mode = !this.roles_edit_mode;
      },
      submit: function () {
        //this.user.roles = this.selectedRoles;
        if (this.user_id > 0) {
          axios.put(this.endpoint + this.user_id, this.user,
            { headers: { "Authorization": `Bearer ${this.JWTToken}` } })
            .then(() => {
              console.log('user saved');
              this.success = true;
              this.edit();
            })
            .catch(err => {
              console.log(err);
              this.error_message = err;
              this.success = false;
            })
        }
        else {
          axios.post(this.endpoint, this.user,
            { headers: { "Authorization": `Bearer ${this.JWTToken}` } })
            .then(response => {
              console.log('user created');
              this.success = true;
              this.user = response.data;
              //this.user_id = this.user.id;
              console.log('user:' + this.user);
              console.log('user.id:' + this.user.id);
              this.edit();
            })
            .catch(err => {
              console.log(err);
              this.error_message = err;
              this.success = false;
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
            this.edit();
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
     }
  }
</script>
<style>
  .vue-form {
    font-size: 16px;
    float:left;
    width: 500px;
    padding: 15px 30px;
    border-radius: 4px;
    margin: 50px auto;
    width: 500px;
    background-color: #fff;
    box-shadow: 0 4px 6px 0 rgba(0, 0, 0, 0.3);
  }
   .error-message p {
    background: #e94b35;
    color: #ffffff;
    font-size: 1.1rem;
    text-align: center;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    border-radius: 0.25em;
    padding: 3px;
  }
  .item_button {
    margin: 2px 2px 2px 2px;
    text-align: center;
    border: groove;
  }
  .item__show {
    min-width: 500px;
    text-align: center;
    align-content: center;
    float: left;
    background-color: antiquewhite;
    margin-bottom: 2px;
  }
  .item__title {
    float: left;
    font-size: 17px;
    font-style: italic;
    font-weight: bold;
    min-width:400px;
  }
  .item__form {
    min-width: 500px;
    text-align: center;
    align-content: center;
    
    background-color: white;
    margin-bottom: 2px;
  }
  li {
    list-style-type: none;
  }

  ul {
    margin-left: 0; 
    padding-left: 0;
  }
</style>
