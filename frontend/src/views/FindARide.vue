<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>Find a ride</h1>
          <p>Here you can view all existing offers</p>

          <v-data-table
            :headers="headers"
            :items="offers"
            hide-default-footer
            :loading="loading"
            class="elevation-1"
          >
          <!-- TODO td to match offer object  -->
            <v-progress-linear v-slot:progress color="blue" indeterminate></v-progress-linear>
            <template v-slot:item.date="{ item }">
              <td>{{ item.date | date }}</td>
            </template>
            <template v-slot:item.spotsAvailable="{ item }">
              <!-- <v-chip :color="getColor(item.spotsAvailable)" dark>{{ item.spotsAvailable }}</v-chip> -->
              <v-chip dark>{{ item.spotsAvailable }}</v-chip>
            </template>
          </v-data-table>
        </v-col>
      </v-row>
    </v-slide-y-transition>

    <v-alert :value="showError" type="error" v-text="errorMessage">
      This is an error alert.
    </v-alert>

    <!-- <v-alert :value="showError" type="warning">
      Are you sure you're using ASP.NET Core endpoint? (default at
      <a href="http://localhost:5000/fetch-data">http://localhost:5000</a
      >)
      <br />
      API call would fail with status code 404 when calling from Vue app
      (default at
      <a href="http://localhost:8080/find-a-ride">http://localhost:8080</a>)
      without devServer proxy settings in vue.config.js file.
    </v-alert> -->
  </v-container>
</template>

<script lang="ts">
// an example of a Vue Typescript component using Vue.extend
import Vue from 'vue';
import { Offer } from '../models/Offer';
import { Location } from '../models/Location';
import { Vehicle } from '../models/Vehicle';
import axios from 'axios';

export default Vue.extend({
  data() {
    return {
      loading: true,
      showError: false,
      errorMessage: 'Error while loading loading offers.',
      offers: [] as Offer[],
      headers: [
        { text: 'Vehicle', value: 'vehicle' },
        { text: 'From', value: 'from' },
        { text: 'To', value: 'to' },
        { text: 'ArrivalTime', value: 'arrivalTime' },
      ],
    };
  },
  methods: {
    // getColor(spotsAvailable: number) {
    //   if ( spotsAvailable === 0) {
    //     return 'red';
    //   } else if ( spotsAvailable == 0.5 && spotsAvailable > 0) {
    //     return 'green';
    //   } else {
    //     return 'red';
    //   }
    // },
    async fetchRides() {
      try {
        const response = await axios.get<Offer[]>('/offers');
        this.offers = response.data;
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while loading rides: ${e.message}.`;
      }
      this.loading = false;
    },
  },
  async created() {
    await this.fetchRides();
  },
});
</script>
