import { Module } from 'vuex';
import { getters } from './getters';
import { actions } from './actions';
import { mutations } from './mutations';
import { OfferState } from './types';
import { RootState } from '../types';

export const state: OfferState = {
  counter: 0,
};

const namespaced: boolean = true;

export const offer: Module<OfferState, RootState> = {
  namespaced,
  state,
  getters,
  actions,
  mutations,
};
