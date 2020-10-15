<template>
  <v-container fluid>
    <v-layout column align-center>
      <img src="@/assets/logo.png" alt="Vuetify.js" class="mb-5">
    </v-layout>
    <v-slide-y-transition mode="out-in">
      <v-row>        
        <v-col>
          <h1 class="headline">Welcome to CarPool</h1>
          <p>This our homepage :)</p>

         <v-btn class="ma-2" color="info" @click.prevent="seedDatabase()">Seed database</v-btn>

          <!-- <ul>
            <li v-for="location in locations" :key="location.id">
              <b>id:</b> {{ location.id }},
              <b>title:</b> {{ location.title }},
              <b>description:</b> {{ location.description }}
            </li>
          </ul> -->
        </v-col>
      </v-row>
    </v-slide-y-transition>


        <v-alert :value="showError" type="error" v-text="errorMessage">
      This is an error alert.
    </v-alert>

  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Location } from '../models/Location';
import axios from 'axios';

@Component({})
export default class Home extends Vue {
  public loading: boolean = true;
  public showError: boolean = false;
  public errorMessage: string = '';
  public locations: Location[] = [];

  public async seedDatabase() {
    try {
      const response = await axios.get<Location[]>('/seed');
      this.locations = response.data;
    } catch (e) {
      this.showError = true;
      this.errorMessage += `Error while loading locations: ${e.message}.`;
    }
    this.loading = false;
  }
}
</script>
