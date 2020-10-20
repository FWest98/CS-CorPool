<template>
  <v-container fluid>
    <v-layout column align-center>
      <img src="@/assets/logo.png" alt="Vuetify.js" class="mb-5">
    </v-layout>
    <h2>Login</h2>
        <form @submit.prevent="handleSubmit">
            <div class="form-group">
                <label for="username">Username</label>
                <input type="text" v-model="username" name="username" class="form-control" :class="{ 'is-invalid': submitted && !username }" />
                <div v-show="submitted && !username" class="invalid-feedback">Username is required</div>
            </div>
            <div class="form-group">
                <label htmlFor="password">Password</label>
                <input type="password" v-model="password" name="password" class="form-control" :class="{ 'is-invalid': submitted && !password }" />
                <div v-show="submitted && !password" class="invalid-feedback">Password is required</div>
            </div>
            <div class="form-group">
                <button class="btn btn-primary" :disabled="loggingIn">Login</button>
                <img v-show="loggingIn" src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
            </div>
        </form>
    
    <v-alert :value="showError" type="error" v-text="errorMessage">
      This is an error alert.
    </v-alert>
  </v-container>
</template>

<script>
export default {
    data() {
        return {
            username: '',
            password: '',
            submitted: false,
            loggingIn: false,
            loading: true,
            showError: false,
            errorMessage: '',
        };
    },
    methods: {
        handleSubmit(e) {
            this.submitted = true;
            if (this.username && this.password) {
                this.getToken();
            }
        },
        async getToken() {
            try {
                const response = await axios.post('/auth', {
                    username: this.username,
                    password: this.password,
                });

                localStorage.setItem('token', JSON.stringify(response.data));
                // console.log('JWT Token\n', response.data);
                this.$router.replace({name: 'home'});
            } catch (e) {
                // console.log(e);
                this.showError = true;
                this.errorMessage += `Error while loading locations: ${e.message}.`;
            }
        },
        async getUserDetails() {
            try {
                const response = await axios.post('/auth', {
                    username: this.username,
                    password: this.password,
                });
                localStorage.setItem('token', response.data);
                this.$router.replace({name: 'offer-a-ride'});

            } catch (e) {
                // console.log(e);
                this.showError = true;
                this.errorMessage += `Error while loading locations: ${e.message}.`;
            }
        },

    },
};
</script>
