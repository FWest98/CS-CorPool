<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>Profile</h1>
          <p>Below can find the information in your the User JSON object</p>

          <v-data-table
            :headers="headers"
            hide-default-footer
            :loading="loading"
            class="elevation-1"
          >
            
            <v-progress-linear v-slot:progress color="blue" indeterminate></v-progress-linear>
            <template v-for="(value, key, index) in user">
                <tr>
                    <td>{{ key }}</td>
                    <td>{{ value }}</td>
                </tr>
            </template>


          </v-data-table>
        </v-col>
      </v-row>
    </v-slide-y-transition>

    <v-alert :value="showError" type="error" v-text="errorMessage">
      This is an error alert.
    </v-alert>

  </v-container>
</template>

<script lang="ts">
// an example of a Vue Typescript component using Vue.extend
import Vue from 'vue';
import { User } from '../models/User';

import axios from 'axios';

export default Vue.extend({
  data() {
    return {
      loading: true,
      showError: false,
      errorMessage: 'Error while loading loading userdata.',
      user: {},
      headers: [
        { text: 'Key', value: 'key' },
        { text: 'Value', value: 'value' },
      ],
    };
  },
  methods: {
    async fetchUser() {
      try {
        const response = await axios.get<User>('/auth');
        this.user = response.data;
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while loading user: ${e.message}.`;
      }
      this.loading = false;
    },
  },
  async created() {
    this.fetchUser();
  },
});
</script>
