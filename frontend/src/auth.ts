
export default function() {
    const tokenString = localStorage.getItem('token');
    if (tokenString === null) {
        return;
    }
    const token = JSON.parse(tokenString);
    if (!token) {
        return;
    }

    // make sure the token is still valid
    const now = new Date();
    // token has expired
    if (now.getTime() > token.expiry) {
        return;
    }
    return {
        headers : {
          Authorization : `Bearer ${token.key}`,
        },
    };
}
