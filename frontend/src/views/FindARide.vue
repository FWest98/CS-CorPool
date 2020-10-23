<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-container>
      <v-row>
        <v-col>
          <h1>Find a ride</h1>
          <p>Here you can find a ride.</p>
            
          <h3>From</h3>
          <input v-model="rideRequest.from.title" placeholder="title">
          <input v-model="rideRequest.from.description" placeholder="description">
          
          <h3>To</h3>
          <input v-model="rideRequest.to.title" placeholder="title">
          <input v-model="rideRequest.to.description" placeholder="description">

          <h3>Arrival Time</h3>
          <datetime v-model="rideRequest.arrivalTime" type="datetime">
          </datetime>

          <v-btn class="ma-2" color="info" @click.prevent="findRide()">Find a ride</v-btn>

        </v-col>

      </v-row>
    
      <v-row>
        <v-col>
            <v-data-table
              :headers="headers"
              :items="offers"
              hide-default-footer
              :loading="loading"
              class="elevation-1"
              disable-pagination
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
      </v-container>
    </v-slide-y-transition>

    <v-alert :value="showError" type="error" v-text="errorMessage">
      This is an error alert.
    </v-alert>

  </v-container>
</template>

<script lang="ts">
// an example of a Vue Typescript component using Vue.extend
import Vue from 'vue';
import { Offer } from '../models/Offer';
import { Location } from '../models/Location';
import { Vehicle } from '../models/Vehicle';
import * as signalR from '@microsoft/signalr';
import auth from '../auth';

export default Vue.extend({
  data() {
    return {
      loading: true,
      showError: false,
      errorMessage: 'Error while loading loading offers.',
      connection: {} as signalR.HubConnection,
      rideRequest: {
          arrivalTime: new Date().toISOString(),
          from: {
              title: '',
              description: '',
          },
          to: {
              title: '',
              description: '',
          },
      },
      offers: [] as Offer[],
      headers: [
        { text: 'User', value: 'user.name' },
        { text: 'From', value: 'from.title' },
        { text: 'To', value: 'to.title' },
        { text: 'Arrival Time', value: 'arrivalTime' },
        { text: 'Vehicle', value: 'vehicle.brand' },
        { text: 'Remaining Capacity', value: 'remainingCapacity' },
      ],
    };
  },
  methods: {
    async findRide() {
      const vue = this;
      // function executed when you click the button
      this.connection
        .invoke("RideRequest", this.rideRequest)
        .catch(function(err) {
            vue.showError = true;
            vue.errorMessage = `Error while connecting to socket: ${err.message}.`;
        });
    },
  },
  async created() {
    const headers = auth();
    if(!headers) return;
    const token = headers.headers.Authorization.replace("Bearer ", "");

    this.connection = new signalR.HubConnectionBuilder()
        .withUrl(`/api/ride/find?access_token=${token}`, {
          transport: signalR.HttpTransportType.WebSockets,
          skipNegotiation: true
        })
        .configureLogging(signalR.LogLevel.Information)
        .withAutomaticReconnect()
        .build();

    const vue = this;
    this.connection.start().catch(err => {
        vue.showError = true;
        vue.errorMessage = `Error establishing connection to socket: ${err.message}.`;
      });

    this.loading = false;
  },

  async mounted() {
    const thisVue = this;
    // thisVue.connection.start();
    thisVue.connection.on("RideResult", function(offer: Offer) {
      thisVue.offers.push(offer);
      // console.log(offer);
    });
  },
});
</script>
