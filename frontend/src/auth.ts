
export default function() {
    const token = localStorage.getItem('token');
    return {
        headers : {
          Authorization : `Bearer ${token}`,
        },
      };

    // // version with expiry checking
    // const token = localStorage.getItem('token');
    // if(!token) return {};

    // // make sure the token is still valid
    // const now = new Date()
    // // token has expired
    // if(now.getTime() > token.expiry){
    //     return {};
    // }
    // return {
    //     headers : {
    //       Authorization : `Bearer ${token.key}`
    //     }
    // }
}
