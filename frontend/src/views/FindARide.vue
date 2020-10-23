<template>
  <v-container fluid>
    <v-slide-y-transition mode="out-in">
      <v-row>
        <v-col>
          <h1>Find a ride</h1>
          <p>Here you can find a ride.</p>

            <div id="divMessages" class="messages"> 
        
            </div>
            
    <h3>From</h3>
    <input v-model="rideRequest.from.title" placeholder="title">
    <input v-model="rideRequest.from.description" placeholder="description">
    
    <h3>To</h3>
    <input v-model="rideRequest.to.title" placeholder="title">
    <input v-model="rideRequest.to.description" placeholder="description">

    <h3>Arrival Time</h3>
    <datetime v-model="rideRequest.arrivalTime" type="datetime" value="2018-05-12T17:19:06.151Z">
      <label for="startDate" slot="before">Label, click to the right</label>
        <span class="description" slot="after">The field description</span>
    </datetime>

            <v-btn class="ma-2" color="info" @click.prevent="findRide()">Find a ride</v-btn>

            <ul v-for="(item, index) in messages" v-bind:key="index + 'itemMessage'">
                <li><b>Name: </b>{{item.user}}</li>
                <li><b>Message: </b>{{item.message}}</li>
            </ul>


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
          arrivalTime: '',
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
            vue.errorMessage = `Error while initiating socket connection: ${err.message}.`;
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
    thisVue.connection.start();
    thisVue.connection.on("RideResult", function(offer: Offer) {
      thisVue.offers.push(offer);
      console.log(offer);
    });
  },
});
</script>
