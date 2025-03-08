
window.logout = async () => {
    const response = await fetch('https://localhost:7294/logout', {
        credentials: 'include'
    });
    return "";
};

window.azureOauth = async (code, redirect_success_uri, redirect_failure_uri) => {
    const response = await fetch('https://localhost:7294/oauth/microsoft', {
        method: "POST",
        credentials: 'include',
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            code: code,
            redirect_success_uri: redirect_success_uri,
            redirect_failure_uri: redirect_failure_uri
        })
    });

}

window.getAllProgram = async ()=> {
    const response = await fetch('https://localhost:7294/program/get/all', {
        method: "GET",
    })

    if (!response.ok){
        return [];
    }

    const content = await  response.json()
    
    try {
        return content
    } catch (error) {
        console.log(error)
        return [];
    }
}
