import { MutationTree } from 'vuex';
import { OfferState } from './types';

export const mutations: MutationTree<OfferState> = {
  incrementCounter(state) {
    state.counter++;
  },
  resetCounter(state) {
    state.counter = 0;
  },
};
