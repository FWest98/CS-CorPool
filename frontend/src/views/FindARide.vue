<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>Find a ride</h1>
          <p>Lorem ipsum</p>

          <v-data-table
            :headers="headers"
            :items="rides"
            hide-default-footer
            :loading="loading"
            class="elevation-1"
          >
            <v-progress-linear v-slot:progress color="blue" indeterminate></v-progress-linear>
            <template v-slot:item.date="{ item }">
              <td>{{ item.date | date }}</td>
            </template>
            <template v-slot:item.spotsAvailable="{ item }">
              <v-chip :color="getColor(item.spotsAvailable)" dark>{{ item.spotsAvailable }}</v-chip>
            </template>
          </v-data-table>
        </v-col>
      </v-row>
    </v-slide-y-transition>

    <v-alert :value="showError" type="error" v-text="errorMessage">
      This is an error alert.
    </v-alert>

    <v-alert :value="showError" type="warning">
      Are you sure you're using ASP.NET Core endpoint? (default at
      <!-- TODO removed hardcoded url, use environment variable or DNS -->
      <a href="http://localhost:5000/fetch-data">http://localhost:5000</a
      >)
      <br />
      API call would fail with status code 404 when calling from Vue app
      (default at
      <a href="http://localhost:8080/find-a-ride">http://localhost:8080</a>)
      without devServer proxy settings in vue.config.js file.
    </v-alert>
  </v-container>
</template>

<script lang="ts">
// an example of a Vue Typescript component using Vue.extend
import Vue from 'vue';
import { Ride } from '../models/Ride';
import axios from 'axios';

export default Vue.extend({
  data() {
    return {
      loading: true,
      showError: false,
      errorMessage: 'Error while loading loading rides.',
      rides: [] as Ride[],
      headers: [
        { text: 'Date', value: 'date' },
        { text: 'From', value: 'from' },
        { text: 'To', value: 'to' },
        { text: 'Spots Available', value: 'spotsAvailable' },
      ],
    };
  },
  methods: {
    getColor(spotsAvailable: number) {
      if ( spotsAvailable === 0) {
        return 'red';
      } else if ( spotsAvailable == 0.5 && spotsAvailable > 0) {
        return 'green';
      } else {
        return 'red';
      }
    },
    async fetchRides() {
      // Date, From (Geo coord.), To (Geo coord. or ID of building), SpotsAvailable, Capacity
      this.rides = [
        {
          date: "\"2014-01-01T23:28:56.782Z\"" ,
          from: "-6.52596, 114.62910, 1.01", // latitude, longitude, distortion
          to: "7.92443, 128.92531, 1.02",
          spotsAvailable: 2,
          capacity: 4
        },
        {
          date: "\"2014-01-01T23:28:56.782Z\"" ,
          from: "18.96129, -173.62121, 1.12", 
          to: "10.44145, 126.66063, 1.03",
          spotsAvailable: 7,
          capacity: 9
        },
      ];

      // try {
      //   const response = await axios.get<Ride[]>('api/rides');
      //   this.rides = response.data;
      // } catch (e) {
      //   this.showError = true;
      //   this.errorMessage = `Error while loading rides: ${e.message}.`;
      // }
      this.loading = false;
    },
  },
  async created() {
    await this.fetchRides();
  },
});
</script>
