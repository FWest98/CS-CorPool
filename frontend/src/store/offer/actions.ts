import { ActionTree } from 'vuex';
import axios from 'axios';
import { OfferState } from './types';
import { RootState } from '../types';

export const actions: ActionTree<OfferState, RootState> = {
  increment({ commit }): any {
    commit('incrementCounter');
  },
  reset({ commit }): any {
    commit('resetCounter');
  },
};
