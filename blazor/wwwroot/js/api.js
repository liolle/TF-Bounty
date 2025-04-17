
window.logout = async (url) => {
  const response = await fetch(`${url}/logout`, {
    credentials: 'include'
  });
  return "";
};

window.azureOauth = async (code, redirect_success_uri, redirect_failure_uri, csrf_token_value,url) => {
  console.log(code,csrf_token_value)
  const response = await fetch(`${url}/oauth/microsoft`, {
    method: "POST",
    credentials: 'include',
    headers: {
      "Content-Type": "application/json",
      "X-CSRF-TOKEN" : csrf_token_value, 
    },
    body: JSON.stringify({
      code: code,
      redirect_success_uri: redirect_success_uri,
      redirect_failure_uri: redirect_failure_uri
    })
  });

}

window.getAllProgram = async (search, url) => {
  const response = await fetch(`${url}/program/get/all${search ? "?search=" + search : ""}`, {
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

window.getUserReport = async (url) => {
  const response = await fetch(`${url}/report/get/me`, {
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


window.getPendingReport = async (url) => {
  const response = await fetch(`${url}/report/get/pending`, {
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

window.validateReport = async (state, id,csrf_token,url) => {
  const response = await fetch(`${url}/report/validate?state=${state}&id=${id}`, {
    credentials: 'include',
    method: "PATCH",
    headers: {
      "X-CSRF-TOKEN" : csrf_token, 
    },
  })

  if (!response.ok) { return false }

  return true
}

window.getProgramById = async (id,url) => {
  const response = await fetch(`${url}/program/get?id=${id}`, {
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


window.addProgram = async (model,csrf_token_value,url) => {
  const response = await fetch(`${url}/program/create`, {
    method: "POST",
    credentials: 'include',
    headers: {
      "Content-Type": "application/json",
      "X-CSRF-TOKEN" : csrf_token_value, 
    },
    body: JSON.stringify(model)
  })

  if (!response.ok) { return false }

  return true
}


window.addRapport = async (model,csrf_token_value,url) => {
  const response = await fetch(`${url}/report/create`, {
    method: "POST",
    credentials: 'include',
    headers: {
      "Content-Type": "application/json",
      "X-CSRF-TOKEN" : csrf_token_value, 
    },
    body: JSON.stringify(model)
  })
}
