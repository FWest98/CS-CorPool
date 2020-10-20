<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>Profile</h1>
          <p>Below can find the information in your the User JSON object</p>

          <v-data-table
            :headers="headers"
            :items="user"
            hide-default-footer
            :loading="loading"
            class="elevation-1"
          >
            
            <v-progress-linear v-slot:progress color="blue" indeterminate></v-progress-linear>
<!-- 

             <template slot="items" slot-scope="props">
              <td v-for="(value, key) in user">{{ props.item.key }} - {{ props.item.value}}</td>
            </template> -->


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
import getConfig from '../auth';

import axios from 'axios';

export default Vue.extend({
  data() {
    return {
      loading: true,
      showError: false,
      errorMessage: 'Error while loading loading userdata.',
      user: [{}],
      headers: [
        { text: 'Key', value: 'key' },
        { text: 'Value', value: 'value' },
      ],
    };
  },
  methods: {
    async fetchUser() {
      try {
        const config = getConfig();
        const response = await axios.get('/auth', config);

        // convert json in to usable format for v-data-table
        for ( const i in response.data ) {
          if ( response.data.hasOwnProperty( i ) ) {
            this.user.push( { key: i, value: response.data[i] } );
          }
        }
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
