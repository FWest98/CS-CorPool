import { GetterTree } from 'vuex';
import { OfferState } from './types';
import { RootState } from '../types';

export const getters: GetterTree<OfferState, RootState> = {
    currentCount(state): number {
        return state.counter;
    },
};
