<template>
  <v-app>

    <v-navigation-drawer persistent :mini-variant="miniVariant" :clipped="clipped" v-model="drawer" enable-resize-watcher fixed app>
      <v-list>
        <v-list-item value="true" v-for="(item, i) in items" :key="i" :to="item.link">
          <v-list-item-action>
            <v-icon v-html="item.icon"></v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title v-text="item.title"></v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-list-item @click="logout()">
         <v-list-item-action>
           <!-- TODO fix the sign out icon -->
            <v-icon>mdi-cancel</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>logout</v-list-item-title>
          </v-list-item-content>
          </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar app :clipped-left="clipped" color="info" dark>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-btn class="d-none d-lg-flex" icon @click.stop="miniVariant = !miniVariant">
        <v-icon v-html="miniVariant ? 'chevron_right' : 'chevron_left'"></v-icon>
      </v-btn>
      <v-btn class="d-none d-lg-flex" icon @click.stop="clipped = !clipped">
        <v-icon>web</v-icon>
      </v-btn>
      <v-toolbar-title v-text="title"></v-toolbar-title>
      <v-spacer></v-spacer>
    </v-app-bar>

    <v-main>
      <router-view/>
    </v-main>

    <v-footer app>
      <span>&nbsp;Web and Cloud Computing&nbsp;&copy;&nbsp;2020</span>
    </v-footer>

  </v-app>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component({})
export default class App extends Vue {
  private clipped: boolean = true;
  private drawer: boolean = true;
  private miniVariant: boolean = false;
  private right: boolean = true;
  private title: string = 'CarPool';
  private items = [
    { title: 'Home', icon: 'home', link: '/' },
    { title: 'Offer a ride', icon: 'mdi-car-multiple', link: '/offer-a-ride' },
    { title: 'Find a ride', icon: 'mdi-account-search', link: '/find-a-ride' },
    { title: 'Profile', icon: 'mdi-account', link: '/profile' },
  ];
  private async logout() {
    localStorage.removeItem('token');
    this.$router.currentRoute.name !== 'login' ?   this.$router.replace({name: 'login'}) : null ;

  }
}
</script>
