
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

window.getAllProgram = async (search) => {
    const response = await fetch(`https://localhost:7294/program/get/all${search ? "?search=" + search : ""}`, {
        method: "GET",
    })

    if (!response.ok) {
        return [];
    }

    const content = await response.json()

    try {
        return content
    } catch (error) {
        console.log(error)
        return [];
    }
}

window.getUserReport = async () => {
    const response = await fetch(`https://localhost:7294/report/get/me`, {
        credentials: 'include',
        method: "GET",
    })

    if (!response.ok) {
        return [];
    }

    const content = await response.json()

    try {
        return content
    } catch (error) {
        console.log(error)
        return [];
    }
}


window.getPendingReport = async () => {
    const response = await fetch(`https://localhost:7294/report/get/pending`, {
        credentials: 'include',
        method: "GET",
    })

    if (!response.ok) {
        return [];
    }

    const content = await response.json()

    try {
        return content
    } catch (error) {
        console.log(error)
        return [];
    }
}

window.validateReport = async (state, id) => {
    const response = await fetch(`https://localhost:7294/report/validate?state=${state}&id=${id}`, {
        credentials: 'include',
        method: "PATCH",
    })

    if (!response.ok) { return false }

    return true
}

window.getProgramById = async (id) => {
    const response = await fetch(`https://localhost:7294/program/get?id=${id}`, {
        method: "GET",
    })

    if (!response.ok) {
        return null;
    }

    const content = await response.json()

    try {
        return content
    } catch (error) {
        console.log(error)
        return null;
    }
}


window.addProgram = async (model) => {
    const response = await fetch(`https://localhost:7294/program/create`, {
        method: "POST",
        credentials: 'include',
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(model)
    })

    if (!response.ok) { return false }

    return true
}


window.addRapport = async (model) => {
    const response = await fetch(`https://localhost:7294/report/create`, {
        method: "POST",
        credentials: 'include',
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(model)
    })
}